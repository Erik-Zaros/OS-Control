<template>
    <q-page class="q-pa-md bg-grey-2 dark:bg-dark">
      <div class="row items-center q-col-gutter-md q-mb-md">
        <div class="col-12 col-md-6">
          <div class="text-h6">Cadastro de Produto</div>
        </div>
        <div class="col-12 col-md-6 text-right">
          <q-btn label="Exportar CSV" icon="download" color="primary" flat @click="exportarCSV" />
        </div>
      </div>
  
      <form-produto @produto-cadastrado="atualizarProdutos" :produtoParaEditar="produtoEdicao" />
  
      <q-separator class="q-my-md" />
  
      <tabela-produtos
        :produtos="produtos"
        @editar-produto="editarProduto"
      />
    </q-page>
  </template>
  
  <script setup>
  /* global erro */
  import { ref, onMounted } from 'vue'
  import axios from 'boot/axios'
  import FormProduto from 'src/components/produtos/FormProduto.vue'
  import TabelaProdutos from 'src/components/produtos/TabelaProdutos.vue'
  
  const produtos = ref([])
  const produtoEdicao = ref(null)
  
  const carregarProdutos = async () => {
    try {
      const { data } = await axios.get('/api/produto/listar')
      produtos.value = data || []
    } catch {
      erro('Erro ao carregar produtos!')
    }
  }
  
  function atualizarProdutos() {
    produtoEdicao.value = null
    carregarProdutos()
  }
  
  function editarProduto(produto) {
    produtoEdicao.value = { ...produto }
  }
  
  function exportarCSV() {
    window.open('/api/produto/exportar-csv', '_blank')
  }

  onMounted(() => {
    produtos.value = [
        { codigo: 'P001', descricao: 'Furadeira X', ativo: true },
        { codigo: 'P002', descricao: 'Serra Circular', ativo: false },
        { codigo: 'P003', descricao: 'Parafusadeira Turbo', ativo: true }
      ]
    })

  // onMounted(carregarProdutos)
  </script>
  