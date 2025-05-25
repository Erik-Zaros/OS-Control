<template>
  <q-form @submit.prevent="handleSubmit" ref="formRef">
    <loading-overlay :loading="loading" />

    <q-card class="q-pa-md q-mb-md" :class="$q.dark.isActive ? 'bg-dark text-white' : 'bg-white'">
      <q-card-section>
        <div class="text-h6">{{ isEdicao ? 'Editar Ticket' : 'Novo Ticket' }}</div>
      </q-card-section>

      <div class="row q-col-gutter-md">
        <div class="col-md-6 col-12">
          <q-input v-model="ticket.titulo" label="Título *" outlined dense maxlength="200"
            :rules="[val => !!val || 'Título é obrigatório']" />
        </div>

        <div class="col-md-6 col-12">
          <q-select v-model="ticket.clienteId" :options="clientes" label="Cliente *" outlined dense option-label="nome"
            option-value="id" emit-value map-options :loading="loadingClientes"
            :rules="[val => !!val || 'Cliente é obrigatório']">
            <template v-slot:no-option>
              <q-item>
                <q-item-section class="text-grey">
                  {{ loadingClientes ? 'Carregando...' : 'Nenhum cliente encontrado' }}
                </q-item-section>
              </q-item>
            </template>
          </q-select>
        </div>

        <div class="col-12">
          <q-input v-model="ticket.descricao" label="Descrição *" outlined type="textarea" rows="3" maxlength="1000"
            :rules="[val => !!val || 'Descrição é obrigatória']" />
        </div>

        <div class="col-md-6 col-12">
          <q-select v-model="ticket.prioridade" :options="prioridades" label="Prioridade" outlined dense emit-value
            map-options />
        </div>

        <div class="col-md-6 col-12" v-if="isEdicao">
          <q-select v-model="ticket.tecnicoId" :options="tecnicos" label="Técnico Responsável" outlined dense
            option-label="nome" option-value="id" emit-value map-options clearable :loading="loadingTecnicos">
            <template v-slot:no-option>
              <q-item>
                <q-item-section class="text-grey">
                  {{ loadingTecnicos ? 'Carregando...' : 'Nenhum técnico encontrado' }}
                </q-item-section>
              </q-item>
            </template>
          </q-select>
        </div>

        <div class="col-md-6 col-12" v-if="isEdicao">
          <q-select v-model="ticket.status" :options="statusOptions" label="Status" outlined dense emit-value
            map-options />
        </div>

        <div class="col-md-6 col-12" v-if="isEdicao && ticket.status === 4">
          <q-input v-model="ticket.dataFechamento" label="Data de Fechamento" type="datetime-local" outlined dense />
        </div>
      </div>

      <div class="q-mt-md q-gutter-sm row items-center">
        <q-space />

        <q-btn flat label="Cancelar" color="grey" @click="cancelar" :disable="loading" />

        <q-btn :label="isEdicao ? 'Atualizar' : 'Criar Ticket'" type="submit" color="primary"
          :icon="isEdicao ? 'edit' : 'add'" :loading="loading" />
      </div>
    </q-card>
  </q-form>
</template>

<script setup>
import { ref, onMounted, watch, computed } from 'vue'
import { useQuasar } from 'quasar'
import { api } from 'boot/axios'
import LoadingOverlay from 'src/components/shared/LoadingOverlay.vue'

const $q = useQuasar()
const emit = defineEmits(['ticket-cadastrado'])

const props = defineProps({
  ticketParaEditar: {
    type: Object,
    default: null
  }
})

const formRef = ref()
const loading = ref(false)
const loadingClientes = ref(false)
const loadingTecnicos = ref(false)

const clientes = ref([])
const tecnicos = ref([])

const prioridades = [
  { label: 'Baixa', value: 0 },
  { label: 'Normal', value: 1 },
  { label: 'Alta', value: 2 }
]

const statusOptions = [
  { label: 'Aberto', value: 0 },
  { label: 'Em Andamento', value: 1 },
  { label: 'Concluído', value: 2 }
]

const resetForm = () => {
  ticket.value = {
    id: null,
    titulo: '',
    descricao: '',
    clienteId: null,
    tecnicoId: null,
    prioridade: 1,
    status: 0,
    dataFechamento: null
  }
}

const ticket = ref({
  id: null,
  titulo: '',
  descricao: '',
  clienteId: null,
  tecnicoId: null,
  prioridade: 2,
  status: 1,
  dataFechamento: null
})

const isEdicao = computed(() => !!ticket.value.id)

async function carregarClientes() {
  loadingClientes.value = true
  try {
    const response = await api.get('/Cliente')
    clientes.value = response.data || []
  } catch (error) {
    console.error('Erro ao carregar clientes:', error)
    $q.notify({
      type: 'negative',
      message: 'Erro ao carregar clientes'
    })
  } finally {
    loadingClientes.value = false
  }
}

async function carregarTecnicos() {
  loadingTecnicos.value = true
  try {
    const response = await api.get('/Usuario/ativos')
    tecnicos.value = response.data?.filter(u => u.tipoUsuario === 'Técnico') || []
  } catch (error) {
    console.error('Erro ao carregar técnicos:', error)
    $q.notify({
      type: 'negative',
      message: 'Erro ao carregar técnicos'
    })
  } finally {
    loadingTecnicos.value = false
  }
}

watch(() => props.ticketParaEditar, (val) => {
  if (val) {
    ticket.value = {
      id: val.id || null,
      titulo: val.titulo || '',
      descricao: val.descricao || '',
      clienteId: val.cliente?.id || null,
      tecnicoId: val.tecnico?.id || null,
      prioridade: val.prioridade ?? 1,
      status: val.status ?? 0,
      dataFechamento: val.dataFechamento ?
        new Date(val.dataFechamento).toISOString().slice(0, 16) : null
    }
  } else {
    resetForm()
  }
}, { immediate: true })

const handleSubmit = async () => {
  try {
    const isValid = await formRef.value.validate()
    if (!isValid) {
      $q.notify({
        type: 'negative',
        message: 'Por favor, corrija os erros no formulário'
      })
      return
    }

    loading.value = true

    const dadosParaEnviar = {
      titulo: ticket.value.titulo,
      descricao: ticket.value.descricao,
      clienteId: ticket.value.clienteId,
      prioridade: ticket.value.prioridade
    }

    if (isEdicao.value) {
      dadosParaEnviar.tecnicoId = ticket.value.tecnicoId
      dadosParaEnviar.status = ticket.value.status
      if (ticket.value.dataFechamento) {
        dadosParaEnviar.dataFechamento = new Date(ticket.value.dataFechamento).toISOString()
      }

      await api.put(`/Ticket/${ticket.value.id}`, dadosParaEnviar)
      $q.notify({
        type: 'positive',
        message: 'Ticket atualizado com sucesso!'
      })
    } else {
      if (ticket.value.tecnicoId) {
        dadosParaEnviar.tecnicoId = ticket.value.tecnicoId
      }

      await api.post('/Ticket', dadosParaEnviar)
      $q.notify({
        type: 'positive',
        message: 'Ticket criado com sucesso!'
      })

      resetForm()
      formRef.value.resetValidation()
    }

    emit('ticket-cadastrado')

  } catch (error) {
    console.error('Erro ao salvar ticket:', error)
    $q.notify({
      type: 'negative',
      message: error.response?.data?.message || 'Erro ao salvar ticket'
    })
  } finally {
    loading.value = false
  }
}

const cancelar = () => {
  resetForm()
  formRef.value.resetValidation()
}

onMounted(() => {
  carregarClientes()
  carregarTecnicos()
})
</script>