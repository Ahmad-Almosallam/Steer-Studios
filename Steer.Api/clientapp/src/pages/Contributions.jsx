import React, { useState, useEffect } from "react";
import { getContributions, getUserInfo } from "../app/api";
import { useNavigate } from "react-router-dom";
import Footer from "../components/Footer";
const Contributions = ({ joinedClanId }) => {
    const [userInfo, setUserInfo] = useState(null);
    const [contributions, setContributions] = useState([]);
    const [errorMessage, setErrorMessage] = useState("");
    const navigate = useNavigate();
    const [loading, setLoading] = useState(true);

    const fetchUserInfo = async () => {
        try {
            const response = await getUserInfo();
            setUserInfo(response.data);
            getContributionsList(response.data.joinedClanId);
            setLoading(false);
        } catch (error) {
            if (error.response && error.response.status === 401) {
                setErrorMessage(
                    "You have been logged out due to logging in from a different device."
                );
                setUserInfo(null);
            }
            setLoading(false);
        }
    };

    const navigateBack = () => {
        navigate("/");
    };

    useEffect(() => {
        fetchUserInfo();
    }, []);

    const getContributionsList = async (clanId) => {
        try {
            const response = await getContributions(clanId);
            setContributions(response.data || []);
        } catch (error) {
            console.error("Failed to fetch contributions:", error);
        }
    };

    if (loading) {
        return <div>Loading...</div>;
    }

    return (
        <div className="container">
            <ul className="contributions">
                {contributions.map((contribution, index) => (
                    <li className="contribution-item" key={index}>
                        {contribution.userName}: {contribution.points} points
                        {contribution.leftAt && ` - Left the clan`}
                    </li>
                ))}
            </ul>
            <button onClick={() => navigateBack()}>Back</button>
            <Footer userName={userInfo.userName}></Footer>
        </div>
    );
};

export default Contributions;
