<template>
  <q-page class="q-pa-md bg-grey-2 dark:bg-dark">
    <div class="text-h6 q-mb-md">Consulta de Tickets</div>

    <div v-if="loading" class="row justify-center q-py-lg">
      <q-spinner-dots color="primary" size="50px" />
    </div>

    <q-banner v-if="error" class="bg-negative text-white q-mb-md" rounded>
      <template v-slot:avatar>
        <q-icon name="warning" />
      </template>
      {{ error }}
      <template v-slot:action>
        <q-btn flat color="white" label="Tentar Novamente" @click="carregarTickets" />
      </template>
    </q-banner>

    <q-card class="q-mb-md">
      <q-card-section>
        <div class="text-subtitle2 q-mb-sm">Filtros</div>
        <div class="row q-gutter-md">
          <q-input v-model="filtros.numero" label="Número do Ticket" outlined dense class="col-md-2 col-xs-12"
            clearable />
          <q-input v-model="filtros.nomeConsumidor" label="Nome do Cliente" outlined dense class="col-md-3 col-xs-12"
            clearable />
          <q-select v-model="filtros.status" :options="statusOptions" label="Status" outlined dense
            class="col-md-2 col-xs-12" clearable emit-value map-options />
          <q-input v-model="filtros.dataInicio" label="Data Início" type="date" outlined dense
            class="col-md-2 col-xs-12" clearable />
          <q-input v-model="filtros.dataFim" label="Data Fim" type="date" outlined dense class="col-md-2 col-xs-12"
            clearable />
        </div>
        <div class="row q-gutter-sm q-mt-sm">
          <q-btn color="primary" label="Filtrar" @click="filtrarOrdens" :loading="loading" />
          <q-btn color="grey-7" label="Limpar Filtros" outline @click="limparFiltros" />
        </div>
      </q-card-section>
    </q-card>

    <div class="row q-gutter-md q-mb-md">
      <q-card class="col">
        <q-card-section class="text-center">
          <div class="text-h4 text-primary">{{ estatisticas.total }}</div>
          <div class="text-caption">Total de Tickets</div>
        </q-card-section>
      </q-card>
      <q-card class="col">
        <q-card-section class="text-center">
          <div class="text-h4 text-orange">{{ estatisticas.abertos }}</div>
          <div class="text-caption">Abertos</div>
        </q-card-section>
      </q-card>
      <q-card class="col">
        <q-card-section class="text-center">
          <div class="text-h4 text-blue">{{ estatisticas.emAndamento }}</div>
          <div class="text-caption">Em Andamento</div>
        </q-card-section>
      </q-card>
      <q-card class="col">
        <q-card-section class="text-center">
          <div class="text-h4 text-green">{{ estatisticas.concluidos }}</div>
          <div class="text-caption">Concluídos</div>
        </q-card-section>
      </q-card>
    </div>

    <TabelaTickets v-if="!loading" :ordens="ordens" :total="totalRegistros" :page="paginacao.page"
      :rows-per-page="paginacao.rowsPerPage" @agendar="abrirModalAgendar" @filtrar="filtrarOrdens" @paginar="onPaginar"
      @editar="editarTicket" @excluir="excluirTicket" @fechar="fecharTicket" />
    <q-dialog v-model="showAgendarModal">
      <q-card style="min-width: 350px;">
        <q-card-section>
          <div class="text-h6">Agendar Ordem de Serviço</div>
          <q-input v-model="novaOS.titulo" label="Título" outlined class="q-mt-sm" />
          <q-input v-model="novaOS.descricao" label="Descrição" outlined class="q-mt-sm" />
          <q-select v-model="novaOS.prioridade" :options="prioridades" label="Prioridade" outlined class="q-mt-sm"
            map-options emit-value />
        </q-card-section>
        <q-card-actions align="right">
          <q-btn flat label="Cancelar" v-close-popup />
          <q-btn flat label="Agendar" color="primary" @click="confirmarAgendamento" />
        </q-card-actions>
      </q-card>
    </q-dialog>
  </q-page>
</template>

<script setup>
import { ref, onMounted } from 'vue'
import { useQuasar } from 'quasar'
import TabelaTickets from 'src/components/tickets/TabelaTicket.vue'
import { api } from 'src/boot/axios'
import { useRouter } from 'vue-router'
const router = useRouter()

const $q = useQuasar()

const ordens = ref([])
const loading = ref(false)
const error = ref(null)
const totalRegistros = ref(0)

const filtros = ref({
  numero: '',
  nomeConsumidor: '',
  status: null,
  dataInicio: '',
  dataFim: ''
})

const paginacao = ref({
  page: 1,
  rowsPerPage: 10
})

const estatisticas = ref({
  total: 0,
  abertos: 0,
  emAndamento: 0,
  concluidos: 0
})

const statusOptions = [
  { label: 'Aberto', value: 'Aberta' },
  { label: 'Em Andamento', value: 'EmAndamento' },
  { label: 'Concluído', value: 'Concluida' },
]

async function carregarTickets() {
  loading.value = true
  error.value = null

  try {
    const params = {
      page: paginacao.value.page,
      pageSize: paginacao.value.rowsPerPage,
      ...filtros.value
    }

    Object.keys(params).forEach(key => {
      if (params[key] === '' || params[key] === null) {
        delete params[key]
      }
    })

    const response = await api.get('/ticket', { params })

    ordens.value = response.data.items || response.data || []
    totalRegistros.value = response.data.totalCount || ordens.value.length

  } catch (err) {
    error.value = 'Erro ao carregar tickets: ' + (err.response?.data?.message || err.message)
    console.error('Erro ao carregar tickets:', err)
  } finally {
    loading.value = false
  }
}

async function carregarEstatisticas() {
  try {
    const response = await api.get('/ticket/dashboard')
    estatisticas.value = {
      total: response.data.totalTickets || 0,
      abertos: response.data.ticketsAbertos || 0,
      emAndamento: response.data.ticketsEmAndamento || 0,
      concluidos: response.data.ticketsConcluidos || 0
    }
  } catch (err) {
    console.error('Erro ao carregar estatísticas:', err)
  }
}

async function filtrarOrdens() {
  paginacao.value.page = 1
  await carregarTickets()
}

function limparFiltros() {
  filtros.value = {
    numero: '',
    nomeConsumidor: '',
    status: null,
    dataInicio: '',
    dataFim: ''
  }
  filtrarOrdens()
}

function onPaginar(props) {
  paginacao.value.page = props.page
  paginacao.value.rowsPerPage = props.rowsPerPage
  carregarTickets()
}

function editarTicket(ticket) {
  router.push(`/dashboard/ticket/${ticket.id}`)
}

async function excluirTicket(ticket) {
  $q.dialog({
    title: 'Confirmar Exclusão',
    message: `Deseja realmente excluir o ticket ${ticket.numero}?`,
    cancel: true,
    persistent: true
  }).onOk(async () => {
    try {
      await api.delete(`/ticket/${ticket.id}`)
      $q.notify({
        type: 'positive',
        message: 'Ticket excluído com sucesso!'
      })
      await carregarTickets()
      await carregarEstatisticas()
    } catch (err) {
      $q.notify({
        type: 'negative',
        message: 'Erro ao excluir ticket: ' + (err.response?.data?.message || err.message)
      })
    }
  })
}

async function fecharTicket(ticket) {
  $q.dialog({
    title: 'Confirmar Fechamento',
    message: `Deseja realmente fechar o ticket ${ticket.numero}?`,
    cancel: true,
    persistent: true
  }).onOk(async () => {
    try {
      await api.patch(`/ticket/${ticket.id}/fechar`)
      $q.notify({
        type: 'positive',
        message: 'Ticket fechado com sucesso!'
      })
      await carregarTickets()
      await carregarEstatisticas()
    } catch (err) {
      $q.notify({
        type: 'negative',
        message: 'Erro ao fechar ticket: ' + (err.response?.data?.message || err.message)
      })
    }
  })
}

const showAgendarModal = ref(false)
const ticketParaAgendar = ref(null)
const novaOS = ref({
  titulo: '',
  descricao: '',
  prioridade: 'Normal'
})

const prioridades = [
  { label: 'Baixa', value: 0 },
  { label: 'Normal', value: 1 },
  { label: 'Alta', value: 2 }
]

function abrirModalAgendar(ticket) {
  ticketParaAgendar.value = ticket
  novaOS.value = {
    titulo: '',
    descricao: '',
    prioridade: 1
  }
  showAgendarModal.value = true
}

async function confirmarAgendamento() {
  if (!novaOS.value.titulo || !novaOS.value.descricao) {
    $q.notify({ type: 'negative', message: 'Preencha todos os campos!' })
    return
  }
  try {
    await api.post('/OrdemServico', {
      titulo: novaOS.value.titulo,
      descricao: novaOS.value.descricao,
      ticketId: ticketParaAgendar.value.id,
      prioridade: novaOS.value.prioridade,
      tecnicoId: null
    })
    $q.notify({ type: 'positive', message: 'Ordem de serviço agendada!' })
    showAgendarModal.value = false
    await carregarTickets()
  } catch (err) {
    $q.notify({ type: 'negative', message: 'Erro ao agendar ordem.' })
    throw err;
  }
}

onMounted(async () => {
  await Promise.all([
    carregarTickets(),
    carregarEstatisticas()
  ])
})
</script>

<style scoped>
.q-card {
  min-height: 80px;
}
</style>