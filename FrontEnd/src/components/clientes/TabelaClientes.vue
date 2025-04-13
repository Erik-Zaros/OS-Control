<template>
  <q-table
    flat
    bordered
    title="Clientes Cadastrados"
    :rows="clientes"
    :columns="columns"
    row-key="cpf"
    rows-per-page-label="Registros por página"
    no-data-label="Nenhum cliente encontrado"
    no-results-label="Nenhum resultado para essa busca"
    separator="horizontal"
    class="shadow-2"
  >
    <template v-slot:body-cell-acoes="props">
      <q-td :props="props" align="center">
        <q-btn dense flat color="warning" icon="edit" @click="$emit('editar-cliente', props.row)" />
      </q-td>
    </template>
  </q-table>
</template>
  
<script setup>
import { defineProps } from 'vue'

defineProps({
  clientes: {
    type: Array,
    default: () => []
  }
})

const columns = [
  { name: 'cpf', label: 'CPF', field: 'cpf', align: 'left', sortable: true },
  { name: 'nome', label: 'Nome', field: 'nome', align: 'left', sortable: true },
  { name: 'cep', label: 'CEP', field: 'cep', align: 'left' },
  { name: 'endereco', label: 'Endereço', field: 'endereco', align: 'left' },
  { name: 'bairro', label: 'Bairro', field: 'bairro', align: 'left' },
  { name: 'numero', label: 'Número', field: 'numero', align: 'left' },
  { name: 'cidade', label: 'Cidade', field: 'cidade', align: 'left' },
  { name: 'estado', label: 'Estado', field: 'estado', align: 'left' },
  { name: 'acoes', label: 'Ações', field: 'acoes', align: 'center' }
]
</script>
  
<style scoped>
.q-table {
  background-color: white;
  border-radius: 8px;
}
</style>
  