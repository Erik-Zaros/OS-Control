<template>
  <q-page class="q-pa-md bg-grey-2 dark:bg-dark">
    <div class="row items-center q-col-gutter-md q-mb-md">
      <div class="col-12 col-md-6">
        <div class="text-h6">Cadastro de Produto</div>
      </div>
    </div>

    <form-produto @produto-cadastrado="atualizarProdutos" :produtoParaEditar="produtoEdicao" />

    <q-separator class="q-my-md" />

    <tabela-produtos :produtos="produtos" @editar-produto="editarProduto" />
  </q-page>
</template>

<script setup>
/* global erro */
import { ref, onMounted } from 'vue'
import { api } from 'boot/axios'
import FormProduto from 'src/components/produtos/FormProduto.vue'
import TabelaProdutos from 'src/components/produtos/TabelaProdutos.vue'

const produtos = ref([])
const produtoEdicao = ref(null)

const carregarProdutos = async () => {
  try {
    const response = await api.get('/produto')
    if (response.data && response.data.success && Array.isArray(response.data.data)) {
      produtos.value = response.data.data || []
    } else {
      produtos.value = Array.isArray(response.data) ? response.data : []
    }
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

onMounted(carregarProdutos)
</script>