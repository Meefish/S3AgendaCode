<script setup lang="ts">
import { onMounted, ref } from 'vue';
import { ResetCalendar } from '../components/API/CalendarApi';
import { DeleteUser, GetAllUsers } from '../components/API/UserApi';
import AddUserPopup from '../components/Popups/AddUserPopup.vue';
import UpdateUserPopup from '../components/Popups/UpdateUserPopup.vue';
import type { User } from '../components/interfaces/UserData/User';
import { UserRole } from '../components/interfaces/UserData/UpdateUser';

const users = ref<User[]>([]);
const error = ref<string | null>(null);
const isAddPopupVisible = ref(false);
const isUpdatePopupVisible = ref(false);
const selectedUser = ref<User | null>(null)

const FetchUsers = async () => {
  try {
    
    users.value = await GetAllUsers();
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
    await ResetCalendar(userId);
  } catch (error: any) {
    error.value = `Error resetting calendar: ${error.message}`;
  }  
};
const HandleDeleteUser = async (userId: number) => {
    try {
      await DeleteUser(userId);
      await FetchUsers(); 
    } catch (error: any) {
      error.value = `Error deleting user: ${error.message}`;
    }
  
};

const RefreshUsers = () => {
  FetchUsers(); 
};

const GetUserRoleName = (roleNumber: UserRole) => {
  return roleNumber === UserRole.Admin ? 'Admin' : 'User';
};

onMounted(FetchUsers);
</script>

<template>
  <main>
    <button @click="HandleAddUser()">Add user</button>
     <div v-if="error" class="error">{{ error }}</div>
     <table>
      <thead>
        <tr>
          <th>UserId</th>
          <th>Username</th>
          <th>Email</th>
          <th>UserRole</th>
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
