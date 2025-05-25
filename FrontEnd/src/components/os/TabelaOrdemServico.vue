<template>
    <q-form @submit.prevent="emitirFiltro">
      <q-card class="q-pa-md q-mb-md">
        <div class="row q-col-gutter-md">
          <div class="col-md-3 col-12">
            <q-input v-model="filtro.numero" label="Número da OS" outlined dense />
          </div>
          <div class="col-md-3 col-12">
            <q-input v-model="filtro.nome" label="Nome do Cliente" outlined dense />
          </div>
          <div class="col-md-3 col-12">
            <q-input v-model="filtro.dataInicio" label="Data Abertura (Início)" type="date" outlined dense />
          </div>
          <div class="col-md-3 col-12">
            <q-input v-model="filtro.dataFim" label="Data Abertura (Fim)" type="date" outlined dense />
          </div>
        </div>
  
        <div class="q-mt-md q-gutter-sm">
          <q-btn label="Filtrar" type="submit" color="primary" />
          <q-btn label="Limpar" @click="limpar" color="secondary" flat />
        </div>
      </q-card>
    </q-form>

    <q-table
      :rows="ordens"
      :columns="columns"
      row-key="numero"
      flat
      bordered
      title="Ordens de Serviço"
      separator="horizontal"
      class="shadow-2"
    >
      <template v-slot:body-cell-acoes="props">
        <q-td :props="props" align="center">
            <q-btn
              color="indigo-8"
              icon="event"
              label="Agendar"
              size="sm"
              unelevated
              class="q-px-sm q-ml-xs"
              @click="abrirModalAgendamento(props.row)">
            </q-btn>
        </q-td>
      </template>
    <template v-slot:body-cell-numero="props">
      <q-td :props="props">
        <q-btn
          flat
          dense
          color="primary"
          class="text-weight-bold"
          :label="props.row.numero"
          @click="verDetalhesOS(props.row.numero)"
        />
      </q-td>
    </template>

    </q-table>

    <q-dialog v-model="agendamentoModal">
      <q-card class="q-pa-md" style="min-width: 400px">
        <div class="text-h6 text-white bg-primary q-pa-sm rounded-borders">
        <q-icon name="event" class="q-mr-sm" />
            Agendar OS #{{ ordemSelecionada?.numero }}
        </div>
        <div class="q-mt-md">
          <q-input v-model="dataAgendamento" label="Data do Agendamento" type="date" dense outlined />
          <q-input v-model="horarioAgendamento" label="Horário" type="time" dense outlined class="q-mt-sm" />
          <q-select
              v-model="tecnicoResponsavel"
              :options="tecnicos"
              label="Técnico Responsável"
              outlined
              dense
              class="q-mt-sm"
              emit-value
              map-options
              clearable
            />
        </div>
  
        <div class="row justify-end q-gutter-sm q-mt-md">
          <q-btn label="Cancelar" flat v-close-popup />
          <q-btn label="Confirmar" color="positive" @click="confirmarAgendamento" />
        </div>
      </q-card>
    </q-dialog>
  </template>  
  
  <script setup>
  import { ref } from 'vue'
  import { useRouter } from 'vue-router'

  const router = useRouter()

  function verDetalhesOS(numero) {
  router.push(`/dashboard/os/${numero}`)
}

  const tecnicos = ref([
  { label: 'João Técnico', value: 'joao' },
  { label: 'Maria Técnica', value: 'maria' },
  { label: 'Carlos Silva', value: 'carlos' }
])
  
  //const = 
  defineProps({
    ordens: {
      type: Array,
      default: () => []
    }
  })
  
  const emit = defineEmits(['filtrar'])
  
  const filtro = ref({
    numero: '',
    nome: '',
    dataInicio: '',
    dataFim: ''
  })

  const emitirFiltro = () => emit('filtrar', filtro.value)
  const limpar = () => {
    filtro.value = { numero: '', nome: '', dataInicio: '', dataFim: '' }
    emitirFiltro()
  }
  
  const agendamentoModal = ref(false)
  const ordemSelecionada = ref(null)
  const dataAgendamento = ref('')
  const horarioAgendamento = ref('')
  const tecnicoResponsavel = ref('')
  
  function abrirModalAgendamento(ordem) {
    ordemSelecionada.value = ordem
    agendamentoModal.value = true
  }
  
  function confirmarAgendamento() {
    console.log('Agendamento:', {
      os: ordemSelecionada.value,
      data: dataAgendamento.value,
      horario: horarioAgendamento.value,
      tecnico: tecnicoResponsavel.value
    })
  
    agendamentoModal.value = false
  }
  
  const columns = [
    { name: 'numero', label: 'Número OS', field: 'numero', align: 'left' },
    { name: 'nome_consumidor', label: 'Cliente', field: 'nome_consumidor', align: 'left' },
    { name: 'cpf_consumidor', label: 'CPF', field: 'cpf_consumidor', align: 'left' },
    { name: 'produto', label: 'Produto', field: 'produto', align: 'left' },
    { name: 'data_abertura', label: 'Data Abertura', field: 'data_abertura', align: 'left' },
    { name: 'status', label: 'Status', field: 'status', align: 'left' },
    { name: 'acoes', label: 'Ações', field: 'acoes', align: 'center' }
    ]
  </script>
  