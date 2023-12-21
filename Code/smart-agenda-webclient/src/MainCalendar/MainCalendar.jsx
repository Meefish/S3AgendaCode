import React from 'react';
import '../CSS/MainCalendar.css';
import DateHandler from './DateHandler.js'; 
import LiveTaskEvent from './WebSocket/WebSocket.jsx';

function MainCalendar() {
    const token = localStorage.getItem('jwtToken');
    return (
        <div>
            <div className="month-full-layout-year">
                <DateHandler token ={token} /> 
                <LiveTaskEvent token={token} />
            </div>
        </div>
    );
}

export default MainCalendar;
