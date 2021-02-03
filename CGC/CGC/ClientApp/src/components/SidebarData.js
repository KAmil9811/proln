import React from 'react';
import * as FaIcons from 'react-icons/fa';
import * as AiIcons from 'react-icons/ai';
import * as IoIcons from 'react-icons/io';
import * as RiIcons from 'react-icons/ri';



export const SidebarData = [
    {
        title: 'Home page',
        path: '/home',
        icon: <AiIcons.AiFillHome />,
    },

    {
        title: 'Your account',
        path: '/userpanel',


    },
    {
        title: 'Control Panel',
        path: '/controlpaneladmin',

    },

    {
        title: 'History',
        path: '#',
        icon: <IoIcons.IoIosPaper />,
        iconClosed: <RiIcons.RiArrowDownSFill />,
        iconOpened: <RiIcons.RiArrowUpSFill />,

        subNav: [
            {
                title: 'Users',
                path: '/user_history',

                cName: 'sub-nav'
            },
            {
                title: 'Orders',
                path: '/order_history',

            },
            {
                title: 'Magazine',
                path: '/glass_history',

                cName: 'sub-nav'
            },
            {
                title: 'Products',
                path: '/ready_glass_history',

            },
            {
                title: 'Machines',
                path: '/all_machine_history',

            }

        ]
    },


    {
        title: 'Orders',
        path: '/orderwarehouse',


    },
    {
        title: 'Magazine',
        path: '/glasswarehouse',


    },
    {
        title: 'Production',
        path: '/selection_of_orders',


    },

    {
        title: 'Saved projects',
        path: '/saved_projects',


    },

    {
        title: 'Products',
        path: '/ready_glass_warehouse',


    },

    {
        title: 'Machines',
        path: '/machinewarehouse',


    },






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