import React, { useState, useEffect } from "react";
import LoginForm from "../components/LoginForm";
import ClanList from "../components/ClanList";
import Footer from "../components/Footer";
//import ClanDetails from "../components/ClanDetails";
import { getUserInfo } from "../app/api";
import ClanDetails from "../components/clanDetails";

const Home = () => {
    const [userInfo, setUserInfo] = useState(null);
    const [token, setToken] = useState(localStorage.getItem("token") || "");
    const [loading, setLoading] = useState(true);
    const [errorMessage, setErrorMessage] = useState("");

    const fetchUserInfo = async () => {
        try {
            const response = await getUserInfo();
            setUserInfo(response.data);
            setLoading(false);
        } catch (error) {
            if (error.response && error.response.status === 401) {
                // Handle unauthorized access
                setErrorMessage(
                    "You have been logged out due to logging in from a different device."
                );
                setUserInfo(null);
            }
            setLoading(false);
        }
    };

    useEffect(() => {
        if (token) {
            fetchUserInfo();
        } else {
            setLoading(false);
        }
    }, []);

    const onLogIn = () => {
        setToken(localStorage.getItem("token"));
        setLoading(true);
        fetchUserInfo();
    };

    const onJoinClan = () => {
        fetchUserInfo();
    };
    const onLeaveClan = () => {
        fetchUserInfo();
    };

    if (loading) {
        return <div>Loading...</div>;
    }

    if (!userInfo) {
        return <LoginForm onLogIn={onLogIn} />;
    }

    return (
        <div className="container">
            {userInfo.joinedClanId ? (
                <ClanDetails
                    clanId={userInfo.joinedClanId}
                    onLeaveClan={onLeaveClan}
                />
            ) : (
                <ClanList onJoinClan={onJoinClan} />
            )}
            <Footer userName={userInfo.userName}></Footer>
        </div>
    );
};

export default Home;
