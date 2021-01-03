// With default styles
import React, { useState } from 'react';
import SideNav, { MenuIcon } from 'react-simple-sidenav';

const MyComponent = (props) => {
    const [showNav, setShowNav] = useState();

    return (
        <div>
            <MenuIcon onClick={() => setShowNav(true)} />
            <SideNav showNav={showNav} onHideNav={() => setShowNav(false)} />
        </div>
    );
};
export default MyComponent;