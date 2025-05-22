<template>
  <q-page class="q-pa-md bg-grey-2 dark:bg-dark">
    <div class="row items-center q-col-gutter-md q-mb-md">
      <div class="col-12 col-md-6">
        <div class="text-h6">Consultar / Exportar Check-in de Ordens de Serviço</div>
      </div>
      <div class="col-12 col-md-6 text-right">
        <q-btn label="Exportar Todos" icon="cloud_download" color="primary" flat @click="exportarTodosCheckin" />
      </div>
    </div>

    <form-filtro-checkin @buscar="filtrarCheckin" />

    <q-separator class="q-my-md" />

    <tabela-checkin :checkins="checkinsFiltrados" @exportar-checkin="exportarCheckin" />
  </q-page>
</template>

<script setup>
import { ref } from 'vue'
import FormFiltroCheckin from 'src/components/checkin/FormFiltroCheckin.vue'
import TabelaCheckin from 'src/components/checkin/TabelaCheckin.vue'

// Dados fictícios
const checkins = ref([
  { os: 1001, tecnico: 'Lucas Alves', status: 'pendente', data: '2024-06-01' },
  { os: 1002, tecnico: 'Mariana Souza', status: 'em_andamento', data: '2024-06-02' },
  { os: 1003, tecnico: 'Carlos Silva', status: 'finalizada', data: '2024-06-03' }
])

const checkinsFiltrados = ref([...checkins.value])

function filtrarCheckin(filtro) {
  checkinsFiltrados.value = checkins.value.filter(os => {
    let match = true
    if (filtro.os && String(os.os) !== String(filtro.os)) match = false
    if (filtro.tecnico && os.tecnico !== getTecnicoNome(filtro.tecnico)) match = false
    if (filtro.status && os.status !== filtro.status) match = false
    if (filtro.dataInicio && os.data < filtro.dataInicio) match = false
    if (filtro.dataFim && os.data > filtro.dataFim) match = false
    return match
  })
}

// Simula exportação de um check-in individual
function exportarCheckin(os) {
  alert(`Check-in da OS ${os.os} exportado para o aplicativo!`)
  // Aqui você faria um POST/PUT para a API do aplicativo
}

// Simula exportação de todos os check-ins filtrados
function exportarTodosCheckin() {
  alert('Todos os check-ins exportados para o aplicativo!')
  // Aqui você faria um POST/PUT para a API do aplicativo
}

// Helper para pegar nome do técnico pelo value
function getTecnicoNome(val) {
  const lista = [
    { value: 1, label: 'Lucas Alves' },
    { value: 2, label: 'Mariana Souza' },
    { value: 3, label: 'Carlos Silva' }
  ]
  const found = lista.find(t => t.value === val)
  return found ? found.label : val
}
</script>
