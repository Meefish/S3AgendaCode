import MainCalendar from "./MainCalendar/MainCalendar";
import Login from "./Login/Login";
import React, {useState, useEffect} from "react";

function App() {
  const [isLoggedIn, setIsLoggedIn] = useState(false);

  useEffect(() => {
    const token = localStorage.getItem('jwtToken');
    if (token) {
      setIsLoggedIn(true);
    }
  }, []);
  
  const handleLogin = () => {
    const token = localStorage.getItem('jwtToken');
    if (token) {
      setIsLoggedIn(true);
    }
  };

  const handleLogout = () => {
    localStorage.removeItem('jwtToken');
    setIsLoggedIn(false);
  };

  return (
    <div>
      {isLoggedIn ? (
        <div>
          <button onClick={handleLogout}>Logout</button>
          <MainCalendar />
        </div>
      ) : (
        <Login onLogin={handleLogin} />
      )}
    </div>
  );
}


export default App;
