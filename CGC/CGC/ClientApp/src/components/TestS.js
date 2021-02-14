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




const title = sessionStorage.getItem('title');

const TestS = () => {

    return (
        <Nav>
            <OutIcon to='/' >
                <FiIcons.FiLogOut />
            </OutIcon>
        </Nav>
    );

}
export default TestS;

