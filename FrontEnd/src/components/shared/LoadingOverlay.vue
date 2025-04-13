<template>
    <transition name="fade">
      <div v-if="loading" class="overlay">
        <q-spinner-ios size="80px" color="white" />
      </div>
    </transition>
  </template>
  
  <script setup>
  defineProps({
    loading: {
      type: Boolean,
      default: false
    }
  })
  </script>
  
  <style scoped>
  .overlay {
    position: fixed;
    top: 0;
    left: 0;
    width: 100vw;
    height: 100vh;
    background-color: rgba(0, 0, 0, 0.6);
    z-index: 9999;
    display: flex;
    align-items: center;
    justify-content: center;
  }
  
  .fade-enter-active,
  .fade-leave-active {
    transition: opacity 0.3s;
  }
  .fade-enter-from,
  .fade-leave-to {
    opacity: 0;
  }
  </style>
  