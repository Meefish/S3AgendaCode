import React, { useState } from 'react';
import { LoginUser } from '../API/LoginApi';

const Login = ({ onLogin }) => {
  const [email, SetEmail] = useState('');
  const [password, SetPassword] = useState('');
  const [error, SetError] = useState('');

  const HandleEmailChange = (e) => SetEmail(e.target.value);
  const HandlePasswordChange = (e) => SetPassword(e.target.value);

  const HandleSubmit = async (e) => {
      e.preventDefault();
      SetError(''); 

      
      try {
        const result = await LoginUser(email, password);
        localStorage.setItem('jwtToken', result.token);
        
        onLogin(result.token);
        console.log('Login successful');
      } catch (error) {
          SetError('Login failed: ' + error.message);
      }
  };

  return (
      <div className="login-container">
          <form onSubmit={HandleSubmit}>
              <h2>Login</h2>
              {error && <p className="error">{error}</p>}
              <div>
                  <label htmlFor="email">Email:</label>
                  <input 
                      type="email" 
                      id="email" 
                      value={email} 
                      onChange={HandleEmailChange} 
                      required 
                  />
              </div>
              <div>
                  <label htmlFor="password">Password:</label>
                  <input 
                      type="password" 
                      id="password" 
                      value={password} 
                      onChange={HandlePasswordChange} 
                      required 
                  />
              </div>
              <button type="submit">Login</button>
          </form>
      </div>
  );
};

export default Login;