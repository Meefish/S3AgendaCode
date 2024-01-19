import MainCalendar from "./Components/MainCalendar/MainCalendar";
import Login from "./Components/Login/Login";
import React, {useState, useEffect} from "react";

function IsTokenExpired(token) {
  try {
    const decoded = JSON.parse(atob(token.split('.')[1]));
    const exp = decoded.exp * 1000; 
    return Date.now() > exp;
  } catch {
    return true; 
  }
}

function App() {
  const [isLoggedIn, SetIsLoggedIn] = useState(false);

  useEffect(() => {
    const token = localStorage.getItem('jwtToken');
    if (token && !IsTokenExpired(token)) {
      SetIsLoggedIn(true);
    }

    const interval = setInterval(() => {
      const token = localStorage.getItem('jwtToken');
      if (!token || IsTokenExpired(token)) {
        localStorage.removeItem('jwtToken');
        SetIsLoggedIn(false);
      }
    }, 60000);

    return () => clearInterval(interval);
  }, []); 

  const HandleLogin = () => {
    const token = localStorage.getItem('jwtToken');
    if (token) {
      SetIsLoggedIn(true);
    }
  };

  const HandleLogout = () => {
    localStorage.removeItem('jwtToken');
    SetIsLoggedIn(false);
  };

  return (
    <div>
      {isLoggedIn ? (
        <div>
          <button onClick={HandleLogout}>Logout</button>
          <MainCalendar />
        </div>
      ) : (
        <Login onLogin={HandleLogin} />
      )}
    </div>
  );
}

export default App;
