<script setup lang="ts">
import { ref } from 'vue';
import { AddUser } from '../API/UserApi'; 
import type { AddUserData } from '../interfaces/UserData/AddUser';

const emit = defineEmits(['close', 'add']);
const token = ref(localStorage.getItem('jwtToken') || '');
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
    if (token){
    await AddUser(newUserData, token.value);
    emit('add');
    emit('close');
    }
  } catch (error) {
    console.error('Failed to add user', error);
  }
};

const HandleClose = () => {
  emit('close');
};
</script>

<template>
  <div class="addUserPopup">
    <button @click="HandleClose">X</button>

  <div class="addUsernameInput">
    <input v-model="username" type="text" placeholder="Username" />
  </div>
  <div class="addEmailInput">
    <input v-model="email" type="email" placeholder="Email" />
  </div>
  <div class="addPasswordInput">
    <input v-model="password" type="password" placeholder="Password" />
  </div>

    <button class="buttonAdd" @click="HandleAdd">Add User</button>
  </div>
</template>

<style scoped>
@import url('https://fonts.googleapis.com/css2?family=Baloo+Bhaijaan+2:wght@500&display=swap');
.addUserPopup {
  position: fixed;
  top: 50%;
  left: 50%;
  transform: translate(-50%, -50%);
  width: 200px;
  padding: 40px;
  background-color: white;
  border: 1px solid black;
  box-shadow: 0px 0px 10px rgba(2, 1, 1, 0.1);
  z-index: 2;
  display: flex;
  flex-direction: column;
  align-items: stretch;
}


.addUserPopup button:first-child {
  position: absolute;
  top: 10px;
  right: 10px;
  background: none;
  border: none;
  cursor: pointer;
  font-weight: bold;
}
.addUserPopup .addUsernameInput,
.addUserPopup .addEmailInput,
.addUserPopup .addPasswordInput {
  display: flex;
  margin-top: 10px; 
  justify-content: center;
}

.buttonAdd{
  margin-top: 35px;
  background-color: #4d4e4d;
  border: none;
  color: white;
  padding: 3px;
  text-align: center;
  cursor: pointer;
  width: 100%;
  font-family: 'Baloo Bhaijaan 2';
}

</style>
