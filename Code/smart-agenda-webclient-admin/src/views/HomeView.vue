<script setup lang="ts">
import { onMounted, ref } from 'vue';
import { ResetCalendar } from '../API/CalendarApi';
import { DeleteUser, GetAllUsers } from '../API/UserApi';
import AddUserPopup from '../components/Popups/AddUserPopup.vue';
import UpdateUserPopup from '../components/Popups/UpdateUserPopup.vue';
import type { User } from '../components/interfaces/UserData/User';
import { UserRole } from '../components/interfaces/UserData/UpdateUser';

const users = ref<User[]>([]);
const error = ref<string | null>(null);
const isAddPopupVisible = ref(false);
const isUpdatePopupVisible = ref(false);
const selectedUser = ref<User | null>(null);
const token = ref(localStorage.getItem('jwtToken') || '');

const FetchUsers = async () => {
  try {
    if (token){
      users.value = await GetAllUsers(token.value);}
  } catch (err: any) {
        error.value = err.message;
      }
    };

const HandleAddUser = () => {
  selectedUser.value = null;
  isAddPopupVisible.value = true;
};

const HandleEditUser = (user: User) => {
  selectedUser.value = user;
  isUpdatePopupVisible.value = true;
  
};

const HandleResetCalendar = async (userId: number) => {
  try {
    if (token){
      await ResetCalendar(userId, token.value);}

  } catch (error: any) {
    error.value = `Error resetting calendar: ${error.message}`;
  }  
};
const HandleDeleteUser = async (userId: number) => {
    try {
      if (token){
      await DeleteUser(userId, token.value);
      await FetchUsers();
    }
    } catch (error: any) {
      error.value = `Error deleting user: ${error.message}`;
    }
  
};

const RefreshUsers = () => {
  FetchUsers(); 
};

const Logout = () => {
  localStorage.removeItem('jwtToken');
  window.location.href = '/';
};

const GetUserRoleName = (roleNumber: UserRole) => {
  return roleNumber === UserRole.Admin ? 'Admin' : 'User';
};

onMounted(FetchUsers);
</script>

<template>
  <button @click="Logout()">Logout</button>
  <button @click="HandleAddUser()">Add user</button>
  <main class="main">
     <div v-if="error" class="error">{{ error }}</div>
     <table class="admin-table">
      <thead>
        <tr>
          <th>UserId</th>
          <th>Username</th>
          <th>Email</th>
          <th>Role</th>
          <th>Actions</th>
        </tr>
      </thead>
      
      <tbody>
        <tr v-for="user in users" :key="user.userId">
          <td>{{ user.userId }}</td>
          <td>{{ user.username }}</td>
          <td>{{ user.email }}</td>
          <td>{{ GetUserRoleName(user.userRole) }}</td>
          <td>
            <button @click="HandleEditUser(user)">Edit</button>
            <button @click="HandleDeleteUser(user.userId)">Delete</button>
            <button @click="HandleResetCalendar(user.userId)">Reset Calendar</button>
          </td>
        </tr>
      </tbody>
    </table>
    <AddUserPopup
      v-if="isAddPopupVisible"
      :user="selectedUser"
      @close="isAddPopupVisible = false"
      @add="RefreshUsers"
    />
    <UpdateUserPopup
      v-if="isUpdatePopupVisible"
      :user="selectedUser"
      @close="isUpdatePopupVisible = false"
      @update="RefreshUsers"
    />
  </main>
</template>

<style scoped>
@import url('https://fonts.googleapis.com/css2?family=Baloo+Bhaijaan+2:wght@500&display=swap');
.main {
  display: flex;
  justify-content: center;
  align-items: center;
  height: 60vh; 
}

.admin-table {
  border-collapse: collapse;
  width: 84%; 

}

.admin-table th, .admin-table td {
  border: 1px solid #000000; 
  font-family: 'Baloo Bhaijaan 2';
  text-align: left;
  padding: 10px;
}

.admin-table th {
  background-color: #f4f4f4e4; 
}

.admin-table tr:nth-child(even) {
  background-color: #f9f9f9e6;
}

.admin-table tr:hover {
  background-color: #eaeaea; 
}
</style>
