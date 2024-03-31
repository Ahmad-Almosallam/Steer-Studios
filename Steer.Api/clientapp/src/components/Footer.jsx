const Footer = ({ userName }) => {
    if (!userName) {
        return null;
    }
    return (
        <div className="footer">
            <span className="loggedIn">Logged In as: {userName}</span>
        </div>
    );
};

export default Footer;
