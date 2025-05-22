<template>
  <q-page class="q-pa-md bg-grey-2 dark:bg-dark">
    <div class="row items-center q-col-gutter-md q-mb-md">
      <div class="col-12 col-md-6">
        <div class="text-h6">Agenda dos Técnicos</div>
      </div>
    </div>

    <div class="bg-white q-pa-md q-mb-md rounded-borders shadow-2">
      <vue-cal
        style="height: 600px"
        :events="events"
        default-view="week"
        @cell-click="onCellClick"
        @event-click="onEventClick"
        :time-from="8 * 60"
        :time-to="18 * 60"
        :disable-views="['years', 'year', 'month', 'day']"
        locale="pt-br"
      />
    </div>

    <q-dialog v-model="showModal">
      <q-card style="min-width:350px;">
        <q-card-section>
          <div class="text-h6">{{ modalData.id ? 'Editar Agendamento' : 'Novo Agendamento' }}</div>
        </q-card-section>
        <q-card-section>
          <q-select
            filled
            v-model="modalData.technician"
            :options="technicians"
            label="Técnico"
            emit-value
            map-options
            class="q-mb-md"
          />
          <q-input
            filled
            v-model="modalData.osNumber"
            label="Nº da Ordem de Serviço"
            type="text"
            class="q-mb-md"
          />
          <q-input
            filled
            v-model="modalData.date"
            label="Data"
            readonly
          />
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
import { ref } from 'vue'
import { useQuasar } from 'quasar'
import VueCal from 'vue-cal'
import 'vue-cal/dist/vuecal.css'

const $q = useQuasar()

const technicians = [
  { label: 'Lucas Alves', value: 1 },
  { label: 'Mariana Souza', value: 2 },
  { label: 'Carlos Silva', value: 3 }
]

const events = ref([
  {
    id: 1,
    title: 'OS 1234 - Lucas Alves',
    start: new Date(new Date().setHours(10, 0, 0, 0)),
    end: new Date(new Date().setHours(11, 0, 0, 0)),
    content: 'Lucas Alves',
    osNumber: '1234',
    technician: 1
  }
])

const showModal = ref(false)
const modalData = ref({
  id: null,
  technician: null,
  osNumber: '',
  date: ''
})

let selectedDate = ref(null)

function onCellClick({ dateTime }) {
  modalData.value = {
    id: null,
    technician: null,
    osNumber: '',
    date: new Date(dateTime).toLocaleString('pt-BR')
  }
  selectedDate.value = dateTime
  showModal.value = true
}

function onEventClick({ event }) {
  modalData.value = {
    id: event.id,
    technician: event.technician,
    osNumber: event.osNumber,
    date: new Date(event.start).toLocaleString('pt-BR')
  }
  selectedDate.value = event.start
  showModal.value = true
}

function saveEvent() {
  if (!modalData.value.technician || !modalData.value.osNumber) {
    $q.notify({ type: 'negative', message: 'Preencha todos os campos!' })
    return
  }
  const tech = technicians.find(t => t.value === modalData.value.technician)
  const start = selectedDate.value
  const end = new Date(new Date(start).getTime() + 60 * 60 * 1000)

  if (modalData.value.id) {
    const idx = events.value.findIndex(ev => ev.id === Number(modalData.value.id))
    if (idx !== -1) {
      events.value[idx] = {
        ...events.value[idx],
        title: `OS ${modalData.value.osNumber} - ${tech.label}`,
        start,
        end,
        technician: tech.value,
        osNumber: modalData.value.osNumber,
        content: tech.label
      }
    }
  } else {
    events.value.push({
      id: events.value.length ? Math.max(...events.value.map(e => Number(e.id))) + 1 : 1,
      title: `OS ${modalData.value.osNumber} - ${tech.label}`,
      start,
      end,
      technician: tech.value,
      osNumber: modalData.value.osNumber,
      content: tech.label
    })
  }
  showModal.value = false
}

</script>
