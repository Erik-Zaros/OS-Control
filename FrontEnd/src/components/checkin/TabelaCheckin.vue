<template>
  <q-table
    flat
    bordered
    title="Ordens de Serviço para Check-in"
    :rows="checkins"
    :columns="columns"
    row-key="os"
    rows-per-page-label="Registros por página"
    no-data-label="Nenhum check-in encontrado"
    no-results-label="Nenhum resultado para essa busca"
    separator="horizontal"
    class="shadow-2"
  >
    <template v-slot:body-cell-acoes="props">
      <q-td :props="props" align="center">
        <q-btn dense flat color="primary" icon="cloud_upload" label="Exportar" @click="$emit('exportar-checkin', props.row)" />
      </q-td>
    </template>
  </q-table>
</template>

<script setup>
import { defineProps } from 'vue'

defineProps({
  checkins: {
    type: Array,
    default: () => []
  }
})

const columns = [
  { name: 'os', label: 'Nº OS', field: 'os', align: 'left', sortable: true },
  { name: 'tecnico', label: 'Técnico', field: 'tecnico', align: 'left' },
  { name: 'status', label: 'Status', field: 'status', align: 'left' },
  { name: 'data', label: 'Data', field: 'data', align: 'left' },
  { name: 'acoes', label: 'Ações', field: 'acoes', align: 'center' }
]
</script>

<style scoped>
.q-table {
  background-color: white;
  border-radius: 8px;
}
</style>
