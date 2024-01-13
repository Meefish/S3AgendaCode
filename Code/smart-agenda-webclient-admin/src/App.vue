<script setup lang="ts">
import { useRouter, RouterView } from 'vue-router'
import { IsTokenExpiredOrValid } from './components/Authentication/AuthCheck';
import { onMounted, onUnmounted } from 'vue';


const router = useRouter();
let intervalId: number;
const startInterval = () => {
  intervalId = setInterval(() => {
    IsTokenExpiredOrValid(router);
  }, 10000);
};

onMounted(() => {
  startInterval();
});

onUnmounted(() => {
  if (intervalId) {
    clearInterval(intervalId);
  }
});
</script>

<template>
  <header>

    <div>

    </div>
  </header>

  <RouterView />
</template>

