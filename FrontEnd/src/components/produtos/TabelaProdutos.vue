<template>
  <q-table
    flat bordered
    title="Produtos Cadastrados"
    :rows="produtos"
    :columns="columns"
    row-key="codigo"
    rows-per-page-label="Registros por página"
    no-data-label="Nenhum produto encontrado"
    no-results-label="Nenhum resultado"
    separator="horizontal"
    class="shadow-2"
  >
    <template v-slot:body-cell-ativo="props">
      <q-td :props="props" align="center">
        <q-icon :name="props.row.ativo ? 'check_circle' : 'cancel'" :color="props.row.ativo ? 'positive' : 'negative'" />
      </q-td>
    </template>

    <template v-slot:body-cell-acoes="props">
      <q-td :props="props" align="center">
        <q-btn dense flat color="warning" icon="edit" @click="$emit('editar-produto', props.row)" />
      </q-td>
    </template>
  </q-table>
</template>

<script setup>
defineProps({
  produtos: { type: Array, default: () => [] }
})

const columns = [
  { name: 'codigo', label: 'Código', field: 'codigo', align: 'left', sortable: true },
  { name: 'descricao', label: 'Descrição', field: 'descricao', align: 'left', sortable: true },
  { name: 'ativo', label: 'Ativo', field: 'ativo', align: 'center' },
  { name: 'acoes', label: 'Ações', field: 'acoes', align: 'center' }
]
</script>
  