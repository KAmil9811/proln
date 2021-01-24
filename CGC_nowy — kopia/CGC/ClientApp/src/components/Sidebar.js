import React, { useState } from 'react';
import styled from 'styled-components';
import { Link } from 'react-router-dom';
import * as FaIcons from 'react-icons/fa';
import * as FiIcons from 'react-icons/fi';
import * as AiIcons from 'react-icons/ai';
import { SidebarData } from './SidebarData';
import SubMenu from './SubMenu';
import { IconContext } from 'react-icons/lib';


const Nav = styled.div`
  background: #15171c;
  height: 80px;
  width: 100%;
  display: flex;
  justify-content: flex-start;
  align-items: center;
  top:0;
  position: fixed;
  z-index: 100;
 

  
`;

const NavIcon = styled(Link)`
  margin-left: 2rem;
  font-size: 2rem;
  height: 80px;
  display: flex;
  justify-content: flex-start;
  align-items: center;
  color: #fff;

`;
const OutIcon = styled(Link)`
 
  font-size: 2.5rem;
  height: 80px;
  display: flex;
  justify-content: flex-end;
  align-items: center;
  right: 2rem;
  position: fixed;
 
  
  color: red;
   
  right: 1 rem;
  color: #fff;
    &:hover{
        color: red;
}
`;

const SidebarNav = styled.nav`
  background: #15171c;
  width: 250px;
  display: flex;
  justify-content: center;
  position: absolute;
  top: 0;
  left: ${({ sidebar }) => (sidebar ? '0' : '-100%')};
  transition: 350ms;
  z-index: 10;
min-height: 100vh;
    height: auto;
`;

const SidebarWrap = styled.div`
  width: 100%;
`;





const Sidebar = () => {
    const [sidebar, setSidebar] = useState(false);

    const showSidebar = () => setSidebar(!sidebar);




    
   

    return (
        <div>
            
            <Nav>
              
                    <NavIcon to='#'>
                         <FaIcons.FaBars onClick={showSidebar} />
                    </NavIcon>
                  

              
                <OutIcon to='/' >
                    <FiIcons.FiLogOut/>
                </OutIcon>
            </Nav>
            <IconContext.Provider value={{ color: '#fff' }}>
                <SidebarNav sidebar={sidebar}>
                    <SidebarWrap>
                        <NavIcon to='#'>
                            <AiIcons.AiOutlineClose onClick={showSidebar} />
                        </NavIcon>
                        {SidebarData.map((item, index) => {
                            return <SubMenu item={item} key={index} />;
                        })}
                    </SidebarWrap>
                </SidebarNav>
            </IconContext.Provider>
        </div>
    );
};

export  default Sidebar;