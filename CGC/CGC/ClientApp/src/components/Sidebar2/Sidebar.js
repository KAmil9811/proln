import React from 'react';

import styled from '@emotion/styled';
import * as FaIcons from 'react-icons/fa';
import * as AiIcons from 'react-icons/ai';
import * as IoIcons from 'react-icons/io';
import * as RiIcons from 'react-icons/ri';
import { SidebarData1 } from './Sidebardata';

const SidebarConteiner = styled.div`
 
  
  float: left;
  height: 100%;
  min-width: 8vw;
  max-width: 200px;
  width: 15vw;
  background: linear-gradient(0deg,black 10%,pink 90%);
  color: #fff;
`

const SidebarHeader = styled.h3`
padding: 20px 0;
text-align:center;
margin-bottom: 10px;
letter-spacing: 6px;


`

const MenuItemConteiner = styled.div``;
const MenuItem = styled.div`
text-align: center;
padding: 6px 20px;
font-weight: 600;
color: rgba(19,15,64);



`;
const Sidebar1 = props => {

    const header = 'CGC'
 
    const menuItemsJSX = SidebarData1.map((item, index) => {
        return (
            <MenuItem key={index}>{item}</MenuItem>
        )
    })
    return (<SidebarConteiner>
        <SidebarHeader>{header}</SidebarHeader>
        <MenuItemConteiner>{menuItemsJSX}</MenuItemConteiner>
       
    </SidebarConteiner>
  )


}
export default Sidebar1