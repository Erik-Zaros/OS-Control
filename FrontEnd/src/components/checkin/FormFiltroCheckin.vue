<template>
  <q-form @submit.prevent="emitirBusca">
    <q-card class="q-pa-md q-mb-md" :class="$q.dark.isActive ? 'bg-dark text-white' : 'bg-white'">
      <div class="row q-col-gutter-md">
        <div class="col-md-4 col-12">
          <q-input v-model="filtro.os" label="Nº OS" outlined dense />
        </div>
        <div class="col-md-4 col-12">
          <q-select
            v-model="filtro.tecnico"
            :options="tecnicos"
            label="Técnico"
            outlined
            dense
            emit-value
            map-options
          />
        </div>
        <div class="col-md-4 col-12">
          <q-select
            v-model="filtro.status"
            :options="statusOS"
            label="Status"
            outlined
            dense
            emit-value
            map-options
          />
        </div>
        <div class="col-md-4 col-12">
          <q-input v-model="filtro.dataInicio" label="Data Inicial" type="date" outlined dense />
        </div>
        <div class="col-md-4 col-12">
          <q-input v-model="filtro.dataFim" label="Data Final" type="date" outlined dense />
        </div>
      </div>
      <div class="q-mt-md">
        <q-btn label="Buscar" color="primary" icon="search" type="submit" />
      </div>
    </q-card>
  </q-form>
</template>

<script setup>
import { ref, defineEmits } from 'vue'

const emit = defineEmits(['buscar'])

const tecnicos = [
  { label: 'Lucas Alves', value: 1 },
  { label: 'Mariana Souza', value: 2 },
  { label: 'Carlos Silva', value: 3 }
]

const statusOS = [
  { label: 'Pendente', value: 'pendente' },
  { label: 'Em andamento', value: 'em_andamento' },
  { label: 'Finalizada', value: 'finalizada' }
]

const filtro = ref({
  os: '',
  tecnico: '',
  status: '',
  dataInicio: '',
  dataFim: ''
})

function emitirBusca() {
  emit('buscar', { ...filtro.value })
}
</script>
