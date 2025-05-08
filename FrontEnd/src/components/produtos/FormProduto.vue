<template>
  <q-form @submit.prevent="handleSubmit">
    <loading-overlay :loading="loading" />

    <q-card class="q-pa-md" :class="$q.dark.isActive ? 'bg-dark text-white' : 'bg-white'">
      <div class="row q-col-gutter-md">
        <div class="col-md-6 col-12">
          <q-input v-model="produto.codigo" label="Código" outlined dense required maxlength="50" />
        </div>
        <div class="col-md-6 col-12">
          <q-input v-model="produto.descricao" label="Descrição" outlined dense required maxlength="120" />
        </div>
      </div>

      <div class="q-mt-md">
        <q-checkbox v-model="produto.ativo" label="Ativo" />
      </div>

      <div class="q-mt-md q-gutter-sm">
        <q-btn label="Gravar" type="submit" color="positive" />
      </div>
    </q-card>
  </q-form>
</template>

<script setup>
/* global sucesso, erro */
import { ref, watch } from 'vue'
import LoadingOverlay from 'src/components/shared/LoadingOverlay.vue'
import { api } from 'boot/axios'

const emit = defineEmits(['produto-cadastrado'])

const props = defineProps({
  produtoParaEditar: {
    type: Object,
    default: null
  }
})

const loading = ref(false)

const produto = ref({
  codigo: '',
  descricao: '',
  ativo: true,
})

const resetForm = () => {
  produto.value = {
    id: null,
    codigo: '',
    descricao: '',
    ativo: true,
    criadoEm: null
  }
}

watch(() => props.produtoParaEditar, (val) => {
  if (val) {
    produto.value = {
      id: val.id || null,
      codigo: val.codigo || '',
      descricao: val.descricao || '',
      ativo: val.ativo !== undefined ? val.ativo : true,
      criadoEm: val.criadoEm || new Date().toISOString()
    }
    console.log('Produto para editar:', produto.value)
  } else {
    resetForm()
  }
}, { immediate: true })

const handleSubmit = async () => {
  loading.value = true
  try {
    const produtoData = {
      id: produto.value.id,
      codigo: produto.value.codigo,
      descricao: produto.value.descricao,
      ativo: produto.value.ativo,
      criadoEm: produto.value.criadoEm || new Date().toISOString()
    }

    if (produto.value.id) {
      const response = await api.put('produto', produtoData)

      if (response.data && response.data.success) {
        sucesso("Produto atualizado com sucesso!")
      } else {
        throw new Error('Falha ao atualizar produto')
      }
    } else {
      delete produtoData.id

      const response = await api.post('produto', produtoData)

      if (response.data && response.data.success) {
        sucesso("Produto cadastrado com sucesso!")
      } else {
        throw new Error('Falha ao cadastrar produto')
      }
    }

    resetForm()
    emit('produto-cadastrado')
  } catch {
    erro('Erro ao cadastrar/atualizar produto.')
  } finally {
    loading.value = false
  }
}

</script>
