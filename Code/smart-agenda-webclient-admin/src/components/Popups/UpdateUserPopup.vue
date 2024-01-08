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
  <div class="UpdateUserPopup">
    <button @click="HandleClose">Close</button>
    <input v-model="username" type="text" placeholder="Username" />
    <input v-model="email" type="email" placeholder="Email" />
    <input v-model="password" type="password" placeholder="Password" />
    <select v-model="userRole">
      <option :value="UserRole.User">User</option>
      <option :value="UserRole.Admin">Admin</option>
    </select>
    <button @click="HandleUpdate">Update User</button>
  </div>
</template>