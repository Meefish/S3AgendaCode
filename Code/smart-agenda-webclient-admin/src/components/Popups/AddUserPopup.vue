<script setup lang="ts">
import { ref } from 'vue';
import { AddUser } from '../API/UserApi'; 
import type { AddUserData } from '../interfaces/UserData/AddUser';

const emit = defineEmits(['close', 'add']);

const username = ref('');
const email = ref('');
const password = ref('');

const HandleAdd = async () => {
  const newUserData: AddUserData = {
    name: username.value,
    email: email.value,
    password: password.value
  };

  try { 
    await AddUser(newUserData);
    emit('add');
    emit('close');
  } catch (error) {
    console.error('Failed to add user', error);
  }
};

const HandleClose = () => {
  emit('close');
};
</script>

<template>
  <div class="AddUserPopup">
    <button @click="HandleClose">Close</button>
    <input v-model="username" type="text" placeholder="Username" />
    <input v-model="email" type="email" placeholder="Email" />
    <input v-model="password" type="password" placeholder="Password" />
    <button @click="HandleAdd">Add User</button>
  </div>
</template>
