<template>
  <q-layout view="lHh Lpr lFf" :class="darkMode ? 'bg-dark text-white' : 'bg-grey-2'">
    <q-page-container>
      <router-view />
    </q-page-container>
  </q-layout>
</template>

<script setup>
import { useQuasar } from 'quasar'
import { ref, watch } from 'vue'

const $q = useQuasar()
const darkMode = ref($q.dark.isActive)

watch(darkMode, (val) => {
  $q.dark.set(val)
})
</script>

<style scoped>
.q-layout {
  transition: background-color 0.3s ease;
}
</style>
