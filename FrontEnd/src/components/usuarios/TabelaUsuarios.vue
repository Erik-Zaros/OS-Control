<template>
  <q-table
    flat
    bordered
    title="Usuários Cadastrados"
    :rows="usuarios"
    :columns="columns"
    row-key="login"
    rows-per-page-label="Records per page"
    no-data-label="Nenhum usuário encontrado"
    no-results-label="Nenhum resultado para essa busca"
    separator="horizontal"
    class="shadow-2"
  >
    <template v-slot:body-cell-ativo="props">
      <q-td :props="props" align="center">
        <q-icon :name="props.row.ativo ? 'check_circle' : 'highlight_off'" :color="props.row.ativo ? 'positive' : 'negative'" />
        <q-toggle
          size="sm"
          v-model="props.row.ativo"
          color="primary"
          @update:model-value="() => $emit('toggle-ativo', props.row)"
        />
      </q-td>
    </template>
    <template v-slot:body-cell-acoes="props">
      <q-td :props="props" align="center">
        <q-btn dense flat color="warning" icon="edit" @click="$emit('editar-usuario', props.row)" />
      </q-td>
    </template>
  </q-table>
</template>

<script setup>
import { defineProps } from 'vue'

defineProps({
  usuarios: {
    type: Array,
    default: () => []
  }
})

const columns = [
  { name: 'nome', label: 'Nome', field: 'nome', align: 'left', sortable: true },
  { name: 'login', label: 'Login', field: 'login', align: 'left', sortable: true },
  { name: 'email', label: 'E-mail', field: 'email', align: 'left' },
  { name: 'telefone', label: 'Telefone', field: 'telefone', align: 'left' },
  { name: 'tipousuario', label: 'Tipo', field: 'tipousuario', align: 'left' },
  { name: 'ultimoacesso', label: 'Último Acesso', field: 'ultimoacesso', align: 'left' },
  { name: 'especialidade', label: 'Especialidade', field: 'especialidade', align: 'left' },
  { name: 'ativo', label: 'Ativo', field: 'ativo', align: 'center' },
  { name: 'acoes', label: 'Ações', field: 'acoes', align: 'center' }
]
</script>

<style scoped>
.q-table {
  background-color: white;
  border-radius: 8px;
}
</style>
