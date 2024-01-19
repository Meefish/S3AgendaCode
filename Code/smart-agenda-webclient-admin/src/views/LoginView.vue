<script lang="ts">
import { defineComponent, ref } from 'vue';
import { useRouter } from 'vue-router';
import { LoginUser } from '../API/LoginApi';

export default defineComponent({
  name: 'Login',
  setup(props, { emit }) {
    const email = ref('');
    const password = ref('');
    const error = ref('');
    const router = useRouter();

    const handleSubmit = async () => {
      error.value = ''; 
      try {
        const { token, isAdmin } = await LoginUser(email.value, password.value);
        emit('onLogin', token);
        if (isAdmin) {
            router.push({ name: 'home' });
        }
      } catch (err: any) {
        error.value = err.message;
      }
    };

    return {
      email,
      password,
      error,
      handleSubmit
    };
  }
});
</script>

<template>
  <div class="login-container">
    <form @submit.prevent="handleSubmit">
      <h2>Login</h2>
      <div v-if="error" class="error">{{ error }}</div>
      <div>
        <label for="email">Email:</label>
        <input 
          type="email" 
          id="email" 
          v-model="email" 
          required 
        />
      </div>
      <div>
        <label for="password">Password:</label>
        <input 
          type="password" 
          id="password" 
          v-model="password" 
          required 
        />
      </div>
      <button type="submit">Login</button>
    </form>
  </div>
</template>


<style scoped>
</style>