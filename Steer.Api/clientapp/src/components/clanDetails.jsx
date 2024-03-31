import React, { useState, useEffect } from "react";
import { useNavigate } from "react-router-dom";
import {
    getClanDetails,
    updateClanPoints,
    leaveClan,
    getUserInfo,
} from "../app/api";

const ClanDetails = ({ clanId, onLeaveClan }) => {
    if (!clanId) return null;
    const navigate = useNavigate();
    const [userInfo, setUserInfo] = useState(null);
    const [currentClanId, setCurrentClanId] = useState(clanId);
    const [clanDetails, setClanDetails] = useState(null);
    const [points, setPoints] = useState();
    useEffect(() => {
        if (clanId !== currentClanId) {
            setCurrentClanId(clanId);
            setClanDetails(null);
        }
        fetchUserInfo();
        fetchClanDetails();
    }, [clanId]);

    const fetchUserInfo = async () => {
        try {
            const response = await getUserInfo();
            setUserInfo(response.data);
        } catch (error) {
            if (error.response && error.response.status === 401) {
                // Handle unauthorized access
                setErrorMessage(
                    "You have been logged out due to logging in from a different device."
                );
                setUserInfo(null);
            }
        }
    };

    const fetchClanDetails = async () => {
        try {
            const response = await getClanDetails(clanId);
            setClanDetails(response.data);
        } catch (error) {
            console.error("Failed to fetch clan details:", error);
        }
    };

    const handleUpdatePoints = async (action) => {
        try {
            const response = await updateClanPoints(clanId, points, action);
            if (response.data) {
                setTimeout(() => {
                    fetchClanDetails(); // Refresh clan details
                }, 1000);
            }
        } catch (error) {
            console.error(`Failed to ${action} points:`, error);
        }
    };

    const handleLeaveClan = async () => {
        try {
            const response = await leaveClan(clanId);
            if (response.data) {
                if (onLeaveClan) {
                    onLeaveClan();
                }
                navigate("/"); // Adjust the route as needed
            }
        } catch (error) {
            console.error("Failed to leave clan:", error);
        }
    };

    const handleShowContributions = async () => {
        navigate("/Contributions");
    };

    return (
        // <div>
        //     <h1>
        //         {clanDetails?.name} - {clanDetails?.totalPoints} points
        //     </h1>
        //     <div>
        //         <input
        //             type="number"
        //             value={points}
        //             onChange={(e) => setPoints(e.target.value)}
        //             placeholder="Points"
        //         />
        //         <button onClick={() => handleUpdatePoints("add")}>
        //             Add Points
        //         </button>
        //         <button onClick={() => handleUpdatePoints("remove")}>
        //             Remove Points
        //         </button>
        //         <button onClick={() => handleUpdatePoints("set")}>
        //             Set Points
        //         </button>
        //     </div>
        //     <button onClick={handleShowContributions}>
        //         Show Current Contributions
        //     </button>
        //     <ul>
        //         {contributions.map((contribution, index) => (
        //             <li key={index}>
        //                 {contribution.userName}: {contribution.points} points
        //                 {contribution.leftAt && ` - Left the clan`}
        //             </li>
        //         ))}
        //     </ul>
        //     <button onClick={handleLeaveClan}>Leave Clan</button>
        // </div>
        <div>
            <div className="header">
                <h1>
                    {clanDetails?.name} - {clanDetails?.totalPoints} points
                </h1>
                <button className="primary" onClick={handleLeaveClan}>
                    Leave Clan
                </button>
            </div>
            <div className="points">Points: {clanDetails?.totalPoints}</div>
            <div className="input-group">
                <input
                    type="number"
                    onChange={(e) => setPoints(e.target.value)}
                    placeholder="Points"
                />
                <button
                    className="primary"
                    onClick={() => handleUpdatePoints("add")}
                >
                    Add Points
                </button>
                <button onClick={() => handleUpdatePoints("remove")}>
                    Remove Points
                </button>
                <button onClick={() => handleUpdatePoints("set")}>
                    Set Points
                </button>
            </div>
            <button onClick={handleShowContributions}>
                Show Current Contributions
            </button>
        </div>
    );
};

export default ClanDetails;
