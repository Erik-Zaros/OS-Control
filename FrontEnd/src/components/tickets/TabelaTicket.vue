<template>
  <q-table :rows="ordens" :columns="columns" row-key="id" flat bordered title="Tickets" separator="horizontal"
    class="shadow-2" :loading="loading">
    <template v-slot:body-cell-id="props">
      <q-td :props="props">
        <q-btn flat dense color="primary" class="text-weight-bold" :label="props.row.id"
          @click="$emit('editar', props.row)" />
      </q-td>
    </template>

    <template v-slot:body-cell-cliente="props">
      <q-td :props="props">
        {{ props.row.cliente?.nome || '-' }}
      </q-td>
    </template>

    <template v-slot:body-cell-cpf="props">
      <q-td :props="props">
        {{ props.row.cliente?.cpfCnpj || '-' }}
      </q-td>
    </template>

    <template v-slot:body-cell-produto="props">
      <q-td :props="props">
        {{ props.row.titulo || '-' }}
      </q-td>
    </template>

    <template v-slot:body-cell-dataAbertura="props">
      <q-td :props="props">
        {{ formatarData(props.row.dataAbertura) }}
      </q-td>
    </template>

    <template v-slot:body-cell-status="props">
      <q-td :props="props">
        {{ statusLabel(props.row.status) }}
      </q-td>
    </template>

    <template v-slot:body-cell-acoes="props">
      <q-td :props="props" align="center">
        <q-btn color="indigo-8" icon="event" label="Agendar" size="sm" unelevated class="q-px-sm q-ml-xs"
          @click="$emit('agendar', props.row)" />
        <q-btn flat color="negative" icon="delete" size="sm" @click="$emit('excluir', props.row)" />
      </q-td>
    </template>
  </q-table>
</template>

<script setup>
defineProps({
  ordens: {
    type: Array,
    default: () => []
  },
  loading: {
    type: Boolean,
    default: false
  }
})

const columns = [
  { name: 'id', label: 'Número do Ticket', field: 'id', align: 'left' },
  { name: 'cliente', label: 'Cliente', field: row => row.cliente?.nome || '-', align: 'left' },
  { name: 'cpf', label: 'CPF', field: row => row.cliente?.cpfCnpj || '-', align: 'left' },
  { name: 'produto', label: 'Produto', field: row => row.titulo || '-', align: 'left' },
  { name: 'dataAbertura', label: 'Data Abertura', field: 'dataAbertura', align: 'left' },
  { name: 'status', label: 'Status', field: 'status', align: 'left' },
  { name: 'acoes', label: 'Ações', field: 'acoes', align: 'center' }
]

function statusLabel(status) {
  switch (status) {
    case 0: return 'Aberto'
    case 1: return 'Em Andamento'
    case 2: return 'Concluído'
    default: return status
  }
}

function formatarData(data) {
  if (!data) return '-'
  const d = new Date(data)
  return d.toLocaleDateString('pt-BR') + ' ' + d.toLocaleTimeString('pt-BR').slice(0, 5)
}
</script>
