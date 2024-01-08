<script setup lang="ts">
import { ref, watch } from 'vue';
import type { User } from '../interfaces/UserData/User';
import { UpdateUser } from '../API/UserApi'; 
import type { UpdateUserData } from '../interfaces/UserData/UpdateUser';
import { UserRole } from '../interfaces/UserData/UpdateUser';


const props = defineProps({
  user: {
    type: Object as () => User | null, 
    default: null 
  }
});

const emit = defineEmits(['close', 'update']);
const token = ref(localStorage.getItem('jwtToken') || '');
const username = ref('');
const email = ref('');
const password = ref('');
const userRole = ref('');

watch(() => props.user, (updateUser) => {
  if (updateUser) {
    username.value = updateUser.username;
    email.value = updateUser.email;
    userRole.value = updateUser.userRole ;
  }
}, { immediate: true });

const HandleUpdate = async () => {

  console.log('Selected user role value:', userRole.value);
  if (props.user){

  let userRoleInt = parseInt(userRole.value, 10);

  const updatedUserData: UpdateUserData = {
    name: username.value,
    email: email.value,
    password: password.value,
    userRole: userRoleInt, 
  };
  try { 
    if (token){
    const response = await UpdateUser(props.user.userId, updatedUserData, token.value); 
    emit('update', response); 
    emit('close');
  }
  } catch (error) {
    console.error('Failed to update user', error);
  }
};
}

const HandleClose = () => {
  emit('close');
};
</script>

<template>
  <div class="updateUserPopup">
    <button @click="HandleClose">X</button>

  <div class="updateUsernameInput">
    <label for="username">Username:</label>
    <input v-model="username" type="text" placeholder="Username" id="username" />
  </div>

  <div class="updateEmailInput">
    <label for="email">Email:</label>
    <input v-model="email" type="email" placeholder="Email" id="email" />
  </div>
  <div class="updatePasswordInput">
    <label for="password">Password:</label>
    <input v-model="password" type="password" placeholder="Password" id="password" />
  </div>

  <div class="updateUserRoleInput">
    <label for="userRole">Role:</label>
    <select v-model="userRole" id="userRole">
      <option :value="UserRole.User">User</option>
      <option :value="UserRole.Admin">Admin</option>
    </select>
  </div>

    <button class="buttonUpdate" @click="HandleUpdate">Update</button>
  </div>
</template>

<style scoped>
@import url('https://fonts.googleapis.com/css2?family=Baloo+Bhaijaan+2:wght@500&display=swap');
.updateUserPopup {
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
  font-family: 'Baloo Bhaijaan 2';
}


.updateUserPopup button:first-child {
  position: absolute;
  top: 10px;
  right: 10px;
  background: none;
  border: none;
  cursor: pointer;
  font-weight: bold;
}

.updateUserPopup .updateEmailInput,
.updateUserPopup .updatePasswordInput,
.updateUserPopup .updateUserRoleInput{
  margin-top: 10px; 
}
.updateUserPopup input {
  width: 100%;
}

.buttonUpdate{
  margin-top: 32px;
  background-color: #4d4e4d;
  border: none;
  color: white;
  padding: 3px;
  text-align: center;
  width: 100%;
  cursor: pointer;
  font-family: 'Baloo Bhaijaan 2', sans-serif;
}

</style>



