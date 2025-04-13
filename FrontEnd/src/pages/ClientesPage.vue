<template>
  <q-page class="q-pa-md bg-grey-2 dark:bg-dark">
    <div class="row items-center q-col-gutter-md q-mb-md">
      <div class="col-12 col-md-6">
        <div class="text-h6">Cadastro de Cliente</div>
      </div>
      <div class="col-12 col-md-6 text-right">
        <q-btn label="Exportar CSV" icon="download" color="primary" flat @click="exportarCSV" />
      </div>
    </div>

    <form-cliente
      @cliente-cadastrado="atualizarClientes"
      :clienteParaEditar="clienteEdicao"
    />

    <q-separator class="q-my-md" />

    <tabela-clientes
      :clientes="clientes"
      @editar-cliente="editarCliente"
    />
  </q-page>
</template>

<script setup>
import { ref } from 'vue'
import FormCliente from 'src/components/clientes/FormCliente.vue'
import TabelaClientes from 'src/components/clientes/TabelaClientes.vue'
import axios from 'boot/axios'

const clienteEdicao = ref(null)
const clientes = ref([
  {
    cpf: '12345678900',
    nome: 'João Silva',
    cep: '17500000',
    endereco: 'Rua A',
    bairro: 'Centro',
    numero: '123',
    cidade: 'Marília',
    estado: 'SP'
  },
  {
    cpf: '98765432100',
    nome: 'Maria Souza',
    cep: '11000000',
    endereco: 'Av B',
    bairro: 'Jardins',
    numero: '456',
    cidade: 'São Paulo',
    estado: 'SP'
  }
])

const carregarClientes = async () => {
  try {
    const response = await axios.get('/api/Cliente/listar')
    clientes.value = response.data || []
  } catch (err) {
    console.error('Erro ao carregar clientes:', err)
  }
}

function atualizarClientes() {
  clienteEdicao.value = null
  carregarClientes()
}

function editarCliente(cliente) {
  clienteEdicao.value = { ...cliente }
}

function exportarCSV() {
  window.open('/api/Cliente/exportar-csv', '_blank')
}
//onMounted(carregarClientes)
</script>
  