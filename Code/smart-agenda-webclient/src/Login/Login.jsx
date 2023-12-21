import React, { useState } from 'react';
import axios from 'axios';

const Login = ({ onLogin }) => {
  const [email, setEmail] = useState('');
  const [password, setPassword] = useState('');
  const [error, setError] = useState('');

  const handleEmailChange = (e) => setEmail(e.target.value);
  const handlePasswordChange = (e) => setPassword(e.target.value);

  const handleSubmit = async (e) => {
      e.preventDefault();
      setError(''); 

      
      try {
        const response = await axios.post('https://localhost:7270/user/login', {
          email,
          password,
      });
      if (response.status === 200 && response.data.token) {
        localStorage.setItem('jwtToken', response.data.token);
        
        onLogin(response.data.token);
        console.log('Login successful');
    } else {
        
        setError('Login failed: ' + response.statusText); 
    }
      } catch (error) {
          setError('Login failed: ' + error.message);
      }
  };

  return (
      <div className="login-container">
          <form onSubmit={handleSubmit}>
              <h2>Login</h2>
              {error && <p className="error">{error}</p>}
              <div>
                  <label htmlFor="email">Email:</label>
                  <input 
                      type="email" 
                      id="email" 
                      value={email} 
                      onChange={handleEmailChange} 
                      required 
                  />
              </div>
              <div>
                  <label htmlFor="password">Password:</label>
                  <input 
                      type="password" 
                      id="password" 
                      value={password} 
                      onChange={handlePasswordChange} 
                      required 
                  />
              </div>
              <button type="submit">Login</button>
          </form>
      </div>
  );
};

export default Login;