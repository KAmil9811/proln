import React from 'react';

import styled from '@emotion/styled';

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
const Sidebar = props => {

    const header = 'CGC'
    const menuItems = ['Home', 'Control_panel']
    return <SidebarConteiner>
        <SidebarHeader>{header}</SidebarHeader>

       
    </SidebarConteiner>


}
export default Sidebar