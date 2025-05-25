<template>
  <q-page class="q-pa-md bg-grey-2 dark:bg-dark">
    <div class="row items-center q-col-gutter-md q-mb-md">
      <div class="col-12 col-md-6">
        <div class="text-h6">Agenda dos Técnicos</div>
      </div>
    </div>
    <div class="bg-white q-pa-md q-mb-md rounded-borders shadow-2">
      <vue-cal style="height: 600px" :events="events" default-view="week" @cell-click="onCellClick"
        @event-click="onEventClick" :time-from="0" :time-to="24 * 60" :disable-views="['years', 'year', 'month', 'day']"
        locale="pt-br" />
    </div>
    <q-dialog v-model="showModal">
      <q-card style="min-width:350px;">
        <q-card-section>
          <div class="text-h6">{{ modalData.id ? 'Editar Agendamento' : 'Novo Agendamento' }}</div>
        </q-card-section>
        <q-card-section>
          <q-select filled v-model="modalData.technician" :options="technicians" label="Técnico" emit-value map-options
            class="q-mb-md" :loading="loadingTechnicians" />
          <q-select filled v-model="modalData.osNumber" :options="ordensServicoOptions" label="Ordem de Serviço"
            emit-value map-options class="q-mb-md" :loading="loadingOS" />
          <q-input filled v-model="modalData.date" label="Data">
            <template v-slot:append>
              <q-icon name="event" class="cursor-pointer">
                <q-popup-proxy transition-show="scale" transition-hide="scale">
                  <q-date v-model="modalData.date" mask="YYYY-MM-DD HH:mm">
                    <div class="row items-center justify-end q-gutter-sm">
                      <q-btn v-close-popup label="OK" color="primary" flat />
                    </div>
                  </q-date>
                </q-popup-proxy>
              </q-icon>
              <q-icon name="access_time" class="cursor-pointer">
                <q-popup-proxy transition-show="scale" transition-hide="scale">
                  <q-time v-model="modalData.date" mask="YYYY-MM-DD HH:mm" format24h>
                    <div class="row items-center justify-end q-gutter-sm">
                      <q-btn v-close-popup label="OK" color="primary" flat />
                    </div>
                  </q-time>
                </q-popup-proxy>
              </q-icon>
            </template>
          </q-input>
        </q-card-section>
        <q-card-actions align="right">
          <q-btn flat label="Cancelar" color="primary" v-close-popup />
          <q-btn flat :label="modalData.id ? 'Salvar' : 'Agendar'" color="primary" @click="saveEvent" />
        </q-card-actions>
      </q-card>
    </q-dialog>
  </q-page>
</template>

<script setup>
import { ref, onMounted } from 'vue'
import { useQuasar } from 'quasar'
import VueCal from 'vue-cal'
import 'vue-cal/dist/vuecal.css'
import { api } from 'src/boot/axios'

const $q = useQuasar()

const events = ref([])
const showModal = ref(false)
const loadingTechnicians = ref(false)
const technicians = ref([])
const loadingOS = ref(false)
const ordensServicoOptions = ref([])

const modalData = ref({
  id: null,
  technician: null,
  osNumber: null,
  date: ''
})

let selectedDate = ref(null)

async function carregarTecnicos() {
  loadingTechnicians.value = true
  try {
    const response = await api.get('/Usuario/tecnicos')
    technicians.value = response.data.map(t => ({
      label: t.nome || t.Nome,
      value: t.id || t.Id
    }))
  } catch (err) {
    $q.notify({ type: 'negative', message: 'Erro ao buscar técnicos!' })
    technicians.value = []
    throw err
  } finally {
    loadingTechnicians.value = false
  }
}

async function carregarOrdensServico() {
  loadingOS.value = true
  try {
    const response = await api.get('/OrdemServico/abertas')
    ordensServicoOptions.value = response.data.map(os => ({
      label: `#${os.id} - ${os.titulo || os.Titulo || ''} (${os.cliente?.nome || os.Cliente?.Nome || ''})`,
      value: os.id
    }))
  } catch (err) {
    $q.notify({ type: 'negative', message: 'Erro ao buscar Ordens de Serviço!' })
    ordensServicoOptions.value = []
    throw err
  } finally {
    loadingOS.value = false
  }
}

async function carregarAgendasTecnicos() {
  try {
    const response = await api.get('/OrdemServico/agendas-tecnicos')

    events.value = response.data.map(item => {
      const startDate = new Date(item.dataExecutar)
      const endDate = new Date(startDate.getTime() + 60 * 60 * 1000)

      return {
        id: item.ordemServicoId,
        start: startDate,
        end: endDate,
        title: `#${item.ordemServicoId} - ${item.titulo} (${item.cliente}) - ${item.tecnicoNome}`,
        technician: item.tecnicoId,
        osNumber: item.ordemServicoId,
        content: item.tecnicoNome,
        class: 'evento-tecnico'
      }
    })

  } catch (err) {
    console.error('Erro detalhado:', err)
    $q.notify({ type: 'negative', message: 'Erro ao carregar agendas dos técnicos!' })
    events.value = []
    throw err
  }
}

function formatarData(dateTime) {
  if (!dateTime) return ''
  const data = typeof dateTime === 'string' ? new Date(dateTime) : dateTime
  if (isNaN(data)) return ''
  const pad = n => n < 10 ? '0' + n : n
  return `${data.getFullYear()}-${pad(data.getMonth() + 1)}-${pad(data.getDate())}T${pad(data.getHours())}:${pad(data.getMinutes())}:00`
}

function onCellClick({ dateTime }) {
  modalData.value = {
    id: null,
    technician: null,
    osNumber: null,
    date: formatarData(dateTime)
  }
  selectedDate.value = dateTime
  showModal.value = true
  Promise.all([carregarTecnicos(), carregarOrdensServico()])
}

function onEventClick({ event }) {
  modalData.value = {
    id: event.id,
    technician: event.technician,
    osNumber: event.osNumber,
    date: formatarData(event.start)
  }
  selectedDate.value = event.start
  showModal.value = true
  Promise.all([carregarTecnicos(), carregarOrdensServico()])
}

async function saveEvent() {
  if (!modalData.value.technician || !modalData.value.osNumber) {
    $q.notify({ type: 'negative', message: 'Preencha todos os campos!' })
    return
  }
  const osId = modalData.value.osNumber
  let dataExecutar = modalData.value.date

  if (dataExecutar && typeof dataExecutar === 'string') {
    if (!dataExecutar.includes('T')) dataExecutar = dataExecutar.replace(' ', 'T')
    if (dataExecutar.length === 16) dataExecutar = dataExecutar + ':00'
  }

  try {
    await api.patch(`/OrdemServico/${osId}/atribuir-tecnico`, {
      tecnicoId: modalData.value.technician,
      dataExecutar: dataExecutar
    })
    $q.notify({ type: 'positive', message: 'Técnico vinculado à OS!' })
    await carregarAgendasTecnicos()
    showModal.value = false
  } catch (err) {
    $q.notify({ type: 'negative', message: 'Erro ao vincular técnico na OS.' })
    throw err
  }
}

onMounted(() => {
  carregarAgendasTecnicos()
})

</script>