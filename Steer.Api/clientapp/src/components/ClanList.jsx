import React, { useState, useEffect } from "react";
import { useNavigate } from "react-router-dom";
import { getClanList, joinClan } from "../app/api";

const ClanList = ({ onJoinClan }) => {
    const [clans, setClans] = useState([]);
    const navigate = useNavigate();

    useEffect(() => {
        const fetchClanList = async () => {
            try {
                const response = await getClanList();
                setClans(response.data || []);
                console.log(response.data);
            } catch (error) {
                console.error("Failed to fetch clan list:", error);
            }
        };

        fetchClanList();
    }, []);

    const handleJoinClan = async (clanId) => {
        try {
            const response = await joinClan(clanId);
            if (response.data) {
                if (onJoinClan) {
                    onJoinClan();
                }
            }
        } catch (error) {
            console.error("Failed to join clan:", error);
        }
    };

    return (
        <div>
            <div className="header">
                <h1>Clans</h1>
            </div>
            <div className="notice">
                You are not a part of the clan, Join a clan.
            </div>
            <ul className="clans-list">
                {clans.map((clan) => (
                    <li className="clan-item" key={clan.id}>
                        {clan.name} - {clan.totalPoints} points
                        <button
                            className="join-button"
                            onClick={() => handleJoinClan(clan.id)}
                        >
                            Join
                        </button>
                    </li>
                ))}
            </ul>
        </div>
    );
};

export default ClanList;
