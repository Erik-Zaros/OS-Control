<template>
  <q-page class="q-pa-md bg-grey-2 dark:bg-dark">
    <div class="row items-center q-col-gutter-md q-mb-md">
      <div class="col-12 col-md-6">
        <div class="text-h6">Cadastro de Cliente</div>
      </div>
    </div>

    <form-cliente @cliente-cadastrado="atualizarClientes" :clienteParaEditar="clienteEdicao" />

    <q-separator class="q-my-md" />

    <tabela-clientes :clientes="clientes" @editar-cliente="editarCliente" />
  </q-page>
</template>

<script setup>
/* global erro */
import { ref, onMounted } from 'vue'
import FormCliente from 'src/components/clientes/FormCliente.vue'
import TabelaClientes from 'src/components/clientes/TabelaClientes.vue'
import { api } from 'boot/axios'

const clienteEdicao = ref(null)
const clientes = ref([])

const carregarClientes = async () => {
  try {
    const response = await api.get('cliente')
    if (response.data && response.data.success && Array.isArray(response.data.data)) {
      clientes.value = response.data.data
    } else {
      clientes.value = Array.isArray(response.data) ? response.data : []
    }
  } catch (error) {
    if (error.response) {
    }
    erro('Erro ao carregar clientes!')
  }
}

function atualizarClientes() {
  clienteEdicao.value = null
  carregarClientes()
}

function editarCliente(cliente) {
  console.log('Editando cliente:', cliente)
  clienteEdicao.value = { ...cliente }
}

onMounted(carregarClientes)
</script>
