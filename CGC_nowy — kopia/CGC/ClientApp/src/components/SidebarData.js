import React from 'react';
import * as FaIcons from 'react-icons/fa';
import * as AiIcons from 'react-icons/ai';
import * as IoIcons from 'react-icons/io';
import * as RiIcons from 'react-icons/ri';

export const SidebarData = [
    {
        title: 'Home',
        path: '/home',
        icon: <AiIcons.AiFillHome />,
    },
    {
        title: 'Control panel',
        path: '/controlpanel',
        icon: <AiIcons.AiFillHome />,
    },
    {
        title: 'Orders',
        path: '/orderwarehouse',
        icon: <AiIcons.AiFillHome />,

    },
    {
        title: 'Glasses',
        path: '/glasswarehouse',
        icon: <AiIcons.AiFillHome />,

    },
    {
        title: 'Production',
        path: '/selection_of_orders',
        icon: <AiIcons.AiFillHome />,

    },
    {
        title: 'Machines',
        path: '/machinewarehouse',
        icon: <AiIcons.AiFillHome />,

    },

    {
        title: 'ReadyProducts',
        path: '/ready_glass_warehouse',
        icon: <AiIcons.AiFillHome />,

    },

    /* {
         title: 'Reports',
         path: '/reports',
         icon: <IoIcons.IoIosPaper />,
         iconClosed: <RiIcons.RiArrowDownSFill />,
         iconOpened: <RiIcons.RiArrowUpSFill />,
 
         subNav: [
             {
                 title: 'Reports',
                 path: '/reports/reports1',
                 icon: <IoIcons.IoIosPaper />,
                 cName: 'sub-nav'
             },
             {
                 title: 'Reports 2',
                 path: '/reports/reports2',
                 icon: <IoIcons.IoIosPaper />,
                 cName: 'sub-nav'
             },
             {
                 title: 'Reports 3',
                 path: '/reports/reports3',
                 icon: <IoIcons.IoIosPaper />
             }
         ]
     },
     */

    /* {
         title: 'Products',
         path: '/products',
         icon: <FaIcons.FaCartPlus />
     },
     {
         title: 'Team',
         path: '/team',
         icon: <IoIcons.IoMdPeople />
     },
     {
         title: 'Messages',
         path: '/messages',
         icon: <FaIcons.FaEnvelopeOpenText />,
 
         iconClosed: <RiIcons.RiArrowDownSFill />,
         iconOpened: <RiIcons.RiArrowUpSFill />,
 
         subNav: [
             {
                 title: 'Message 1',
                 path: '/messages/message1',
                 icon: <IoIcons.IoIosPaper />
             },
             {
                 title: 'Message 2',
                 path: '/messages/message2',
                 icon: <IoIcons.IoIosPaper />
             }
         ]
     },
     {
         title: 'Support',
         path: '/support',
         icon: <IoIcons.IoMdHelpCircle />
     }
     */
];