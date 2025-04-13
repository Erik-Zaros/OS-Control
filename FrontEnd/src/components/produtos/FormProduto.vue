<template>
  <q-form @submit.prevent="handleSubmit">
    <loading-overlay :loading="loading" />

    <q-card class="q-pa-md" :class="$q.dark.isActive ? 'bg-dark text-white' : 'bg-white'">
      <div class="row q-col-gutter-md">
        <div class="col-md-6 col-12">
          <q-input
            v-model="produto.codigo"
            label="Código"
            outlined
            dense
            required
            maxlength="50"
          />
        </div>
        <div class="col-md-6 col-12">
          <q-input
            v-model="produto.descricao"
            label="Descrição"
            outlined
            dense
            required
            maxlength="120"
          />
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
import axios from 'axios'

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
  ativo: true
})

watch(() => props.produtoParaEditar, (val) => {
  if (val) {
    produto.value = { ...val }
  }
}, { immediate: true })

const handleSubmit = async () => {
  loading.value = true
  try {
    if (props.produtoParaEditar) {
      await axios.put(`/api/produtos/${produto.value.codigo}`, produto.value)
      sucesso("Produto atualizado com sucesso!")
    } else {
      await axios.post('/api/produtos', produto.value)
      sucesso("Produto cadastrado com sucesso!")
    }

    emit('produto-cadastrado')
    produto.value = {
      codigo: '',
      descricao: '',
      ativo: true
    }
  } catch {
    erro('Erro ao cadastrar/atualizar produto.')
  } finally {
    loading.value = false
  }
}

</script>
