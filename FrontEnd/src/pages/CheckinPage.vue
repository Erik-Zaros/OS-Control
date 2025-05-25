<template>
  <q-page class="q-pa-md bg-grey-2 dark:bg-dark">
    <div class="row items-center q-col-gutter-md q-mb-md">
      <div class="col-12 col-md-6">
        <div class="text-h6">Consultar / Exportar Check-in de Ordens de Serviço</div>
      </div>
      <div class="col-12 col-md-6 text-right">
        <q-btn label="Exportar Todos" icon="cloud_download" color="primary" flat @click="exportarTodosCheckin"
          :loading="exportandoTodos" />
      </div>
    </div>

    <form-filtro-checkin @buscar="filtrarCheckin" :tecnicos="tecnicos" :loading-tecnicos="loadingTechnicians" />

    <q-separator class="q-my-md" />

    <tabela-checkin :checkins="checkinsFiltrados" @exportar-checkin="exportarCheckin" :loading="loadingCheckins" />
  </q-page>
</template>

<script setup>
import { ref, onMounted } from 'vue'
import { useQuasar } from 'quasar'
import { api } from 'src/boot/axios'
import FormFiltroCheckin from 'src/components/checkin/FormFiltroCheckin.vue'
import TabelaCheckin from 'src/components/checkin/TabelaCheckin.vue'

const $q = useQuasar()

const checkins = ref([])
const checkinsFiltrados = ref([])
const tecnicos = ref([])
const loadingCheckins = ref(false)
const loadingTechnicians = ref(false)
const exportandoTodos = ref(false)

async function carregarTecnicos() {
  loadingTechnicians.value = true
  try {
    const response = await api.get('/Usuario/tecnicos')
    tecnicos.value = response.data.map(t => ({
      label: t.nome || t.Nome,
      value: t.id || t.Id
    }))
  } catch (err) {
    console.error('Erro ao carregar técnicos:', err)
    $q.notify({ type: 'negative', message: 'Erro ao buscar técnicos!' })
    tecnicos.value = []
  } finally {
    loadingTechnicians.value = false
  }
}

async function carregarCheckins() {
  loadingCheckins.value = true
  try {
    const response = await api.get('/OrdemServico')
    checkins.value = response.data
      .filter(item => item.status === 0)
      .map(item => ({
        os: item.id,
        tecnico: item.tecnico?.nome || item.tecnico?.Nome || 'Não atribuído',
        tecnicoId: item.tecnico?.id || item.tecnico?.Id || null,
        status: getStatusDisplay(item.status),
        statusOriginal: item.status,
        data: item.dataExecutar || item.dataCriacao || item.data,
        dataFormatada: formatarDataBr(item.dataExecutar || item.dataCriacao || item.data),
        cliente: item.cliente?.nome || item.cliente?.Nome || 'Cliente não informado',
        titulo: item.titulo || item.descricao || 'Sem título'
      }))
    checkinsFiltrados.value = [...checkins.value]
  } catch (err) {
    console.error('Erro ao carregar ordens de serviço:', err)
    $q.notify({ type: 'negative', message: 'Erro ao carregar ordens de serviço!' })
    checkins.value = []
    checkinsFiltrados.value = []
  } finally {
    loadingCheckins.value = false
  }
}

function getStatusDisplay(status) {
  const statusMap = {
    0: 'Aguardando exportação',
    1: 'Em andamento',
    2: 'Concluída',
    3: 'Exportada para Checkin'
  }
  return statusMap[status] || 'Pendente'
}

function filtrarCheckin(filtro) {
  checkinsFiltrados.value = checkins.value.filter(os => {
    let match = true

    if (filtro.os && String(os.os) !== String(filtro.os)) {
      match = false
    }

    if (filtro.tecnico && os.tecnicoId !== filtro.tecnico) {
      match = false
    }

    if (filtro.status && os.status !== filtro.status) {
      match = false
    }

    if (filtro.dataInicio) {
      const dataOS = new Date(os.data)
      const dataInicio = new Date(filtro.dataInicio)
      if (dataOS < dataInicio) match = false
    }

    if (filtro.dataFim) {
      const dataOS = new Date(os.data)
      const dataFim = new Date(filtro.dataFim)
      if (dataOS > dataFim) match = false
    }

    return match
  })
}

function formatarDataBr(isoString) {
  if (!isoString) return ''
  const date = new Date(isoString)
  if (isNaN(date)) return isoString
  const pad = n => (n < 10 ? '0' + n : n)
  return `${pad(date.getDate())}/${pad(date.getMonth() + 1)}/${date.getFullYear()} ${pad(date.getHours())}:${pad(date.getMinutes())}`
}

async function exportarCheckin(os) {
  try {
    await api.patch(`/OrdemServico/${os.os}/exportar`)

    const index = checkins.value.findIndex(item => item.os === os.os)
    if (index !== -1) {
      checkins.value[index].status = 'exportado'
    }

    const indexFiltrado = checkinsFiltrados.value.findIndex(item => item.os === os.os)
    if (indexFiltrado !== -1) {
      checkinsFiltrados.value[indexFiltrado].status = 'exportado'
    }

    $q.notify({ type: 'positive', message: `OS ${os.os} exportada para o aplicativo!` })
  } catch (err) {
    console.error('Erro ao exportar OS:', err)
    $q.notify({ type: 'negative', message: `Erro ao exportar a OS ${os.os}!` })
  }
}

async function exportarTodosCheckin() {
  const osParaExportar = checkinsFiltrados.value.filter(os => os.status !== 'exportado')

  if (osParaExportar.length === 0) {
    $q.notify({ type: 'info', message: 'Nenhuma OS pendente para exportar!' })
    return
  }

  exportandoTodos.value = true
  let sucessos = 0
  let erros = 0

  try {
    const loteSize = 5
    for (let i = 0; i < osParaExportar.length; i += loteSize) {
      const lote = osParaExportar.slice(i, i + loteSize)

      await Promise.allSettled(
        lote.map(async (os) => {
          try {
            await api.patch(`/OrdemServico/${os.os}/exportar`)

            const indexOriginal = checkins.value.findIndex(item => item.os === os.os)
            if (indexOriginal !== -1) {
              checkins.value[indexOriginal].status = 'exportado'
            }

            const indexFiltrado = checkinsFiltrados.value.findIndex(item => item.os === os.os)
            if (indexFiltrado !== -1) {
              checkinsFiltrados.value[indexFiltrado].status = 'exportado'
            }

            sucessos++
          } catch (err) {
            console.error(`Erro ao exportar OS ${os.os}:`, err)
            erros++
          }
        })
      )
    }

    if (erros === 0) {
      $q.notify({
        type: 'positive',
        message: `${sucessos} check-ins exportados com sucesso!`
      })
    } else {
      $q.notify({
        type: 'warning',
        message: `${sucessos} exportados com sucesso, ${erros} com erro.`
      })
    }

  } catch (err) {
    console.error('Erro geral na exportação:', err)
    $q.notify({ type: 'negative', message: 'Erro durante a exportação em massa!' })
  } finally {
    exportandoTodos.value = false
  }
}

onMounted(async () => {
  try {
    await Promise.all([
      carregarTecnicos(),
      carregarCheckins()
    ])
  } catch (err) {
    console.error('Erro na inicialização:', err)
  }
})
</script>
