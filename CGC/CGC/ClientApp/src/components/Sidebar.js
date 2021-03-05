import { MDBScrollbar} from 'mdbreact';
import React, { useState } from 'react';
import styled from 'styled-components';
import { Link } from 'react-router-dom';
import * as FaIcons from 'react-icons/fa';
import * as FiIcons from 'react-icons/fi';
import * as AiIcons from 'react-icons/ai';
import { SidebarData } from './SidebarData';
import { SidebarData2 } from './SidebarData2';
import SubMenu from './SubMenu';
import { IconContext } from 'react-icons/lib';
import './second.css';
import "./scrollbar.css";


const Titleee = styled.div`
  background: #15171c;
  height: 10px;
  width: 100%;
  display: flex;
  color: #ffffff;
  justify-content: flex-start;
  align-items: center;
  top:0;
  position: fixed;
  z-index: 100;
 
`

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
  position: fixed;
  top: 80px;
  left: ${({ sidebar }) => (sidebar ? '0' : '-100%')};
  transition: 350ms;
  z-index: 10;
    height: 100%;
overflow-y: scroll;
`;

const SidebarWrap = styled.div`
  width: 100%;
    margin-top: 80px;
`;



const title = sessionStorage.getItem('title');
const scrollContainerStyle = {maxHeight: "400px" };

const Sidebar = () => {
    const [sidebar, setSidebar] = useState(false);
    const [sidebar2, setSidebar2] = useState(true);

    const showSidebar = () => {
        setSidebar(!sidebar);
        setSidebar2(!sidebar2);
    }
    if (sidebar == true) {
        return (
            <div>
                <IconContext.Provider value={{ color: '#fff' }}>
                    <Nav>
                        <NavIcon to='#'>
                            <FaIcons.FaBars onClick={showSidebar} />
                        </NavIcon>

                        <OutIcon to='/' >
                            <FiIcons.FiLogOut />
                        </OutIcon>
                    </Nav>
                    <SidebarNav sidebar={sidebar}>
                        <SidebarWrap>
                            

                                {SidebarData.map((item, index) => {
                                    return <SubMenu item={item} key={index} />;
                                })}

                            

                        </SidebarWrap>
                    </SidebarNav>
                </IconContext.Provider>
        </div>
        );
    }
    else {
        return (
            <div>
                <IconContext.Provider value={{ color: '#fff' }}>
                    <Nav>
                        <NavIcon to='#'>
                            <FaIcons.FaBars onClick={showSidebar} />
                        </NavIcon>
                        <OutIcon to='/' >
                            <FiIcons.FiLogOut />
                        </OutIcon>

                    </Nav>

                    <SidebarNav sidebar={sidebar2} className="second">
                        <SidebarWrap >
                            {SidebarData2.map((item, index) => {
                                return <SubMenu item={item} key={index} />;
                            })}
                        </SidebarWrap>
                    </SidebarNav>
                </IconContext.Provider>
            </div>
        );
    }
};

export default Sidebar;