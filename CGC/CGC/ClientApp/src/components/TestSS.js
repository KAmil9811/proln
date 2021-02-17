import React, { useState } from 'react';
import styled from 'styled-components';
import { Link } from 'react-router-dom';
import * as FaIcons from 'react-icons/fa';
import * as FiIcons from 'react-icons/fi';
import * as AiIcons from 'react-icons/ai';
import { SidebarData } from './SidebarData';
import SubMenu from './SubMenu';
import { IconContext } from 'react-icons/lib';
import './Sidebar.css'







const SidebarNav = styled.nav`
  background: #15171c;
  width: 250px;
  display: flex;
  justify-content: center;
  position: absolute;
  margin-top: 0;

  
  transition: 350ms;
  z-index: 100;
min-height: 100vh;
    height: auto;
    height: 100%;
`;

const SidebarWrap = styled.div`
  width: 100%;
`;

const Sid = styled.div`
 float:left;
 margin-top: 80px;

`;



const title = sessionStorage.getItem('title');


const TestSS = () => {




    return (
          <Sid>
            <IconContext.Provider value={{ color: '#fff' }}>
                <SidebarNav>
                    <SidebarWrap>
                    
                        {SidebarData.map((item, index) => {
                            return <SubMenu item={item} key={index} />;
                        })}
                    </SidebarWrap>
                </SidebarNav>
            </IconContext.Provider>
       
            
        </Sid>
            );

}
export default TestSS;









