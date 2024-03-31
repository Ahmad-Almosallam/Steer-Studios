import React, { useState } from "react";
import { useLocation } from "react-router-dom";
import { login } from "../app/api";

const LoginForm = ({ onLogIn }) => {
    const location = useLocation();
    const query = new URLSearchParams(location.search);
    const msg = query.get("msg");
    const [username, setUsername] = useState("");

    const handleLogin = async () => {
        try {
            const response = await login(username);
            if (response.data) {
                onLogIn();
            }
        } catch (error) {
            console.log(error);
        }
    };

    return (
        <div className="login-container">
            {msg === "loggedOut" && (
                <p className="logout-message">
                    You have been logged out due to logging in from a different device.
                </p>
            )}
            <h1>Login</h1>
            <div className="input-group">
                <label htmlFor="username-input">Enter Username</label>
                <input
                    id="username-input"
                    type="text"
                    value={username}
                    onChange={(e) => setUsername(e.target.value)}
                    placeholder="Enter username"
                    className="form-control"
                />
                <button className="login-button" onClick={handleLogin}>Login</button>
            </div>
        </div>
    );
};

export default LoginForm;
