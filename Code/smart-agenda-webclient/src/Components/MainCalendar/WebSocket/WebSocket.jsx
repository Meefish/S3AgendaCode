import React, { useState, useEffect } from 'react';
import '../../../Assets/CSS/DateHandler.css';
import {jwtDecode} from 'jwt-decode';


const WebSocket_URL = process.env.REACT_APP_WEBSOCKET_BASE_URL;

function LiveTaskEvent({token}) {     // eslint-disable-next-line
    const [webSocket, SetWebSocket] = useState(null);  
    const [task, SetTask] = useState(null);
    const [showPopup, SetShowPopup] = useState(false);

    const GetCalendarIdFromToken = (token) => {
        try {
          const decodedToken = jwtDecode(token);
          return decodedToken.calendarId;
        } catch (error) {
          console.error("Error decoding the JWT token", error);
          return null;
        }
      };
    useEffect(() => {

          const calendarId = GetCalendarIdFromToken(token);

        const ws = new WebSocket(WebSocket_URL);
        SetWebSocket(ws);

        ws.onopen = () => {
            console.log("WebSocket Connected");
            ws.send(JSON.stringify({ calendarId }));
        };

        ws.onmessage = (event) => {
            console.log("Message received:", event.data);
            try {
                const receivedTasks = JSON.parse(event.data);
                if (Array.isArray(receivedTasks) && receivedTasks.length > 0) {
                    const latestTask = receivedTasks[receivedTasks.length - 1];
                    SetTask(latestTask);
                    SetShowPopup(true); 
                }
            } catch (error) {
                console.error("Task error:", error);
            }

        };

        ws.onclose = () => {
            console.log("WebSocket closed");

        };

        return () => {
            ws.close(); 
        };
    },[token]);

    const ClosePopup = () => {
        SetShowPopup(false);
    };  
    
    return (
        <div>
        {showPopup && task && (
            <div className="eventpopup" onClick={ClosePopup}>
                It's time for: {task.TaskName} {task.DueDate.split('T')[1].substring(0, 5)}
            </div>
        )}
    </div>
);
}

export default LiveTaskEvent;