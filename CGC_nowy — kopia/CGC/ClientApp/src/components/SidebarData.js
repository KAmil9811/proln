import React from 'react';
import * as FaIcons from 'react-icons/fa';
import * as AiIcons from 'react-icons/ai';
import * as IoIcons from 'react-icons/io';
import * as RiIcons from 'react-icons/ri';



export const SidebarData = [
    {
        title: 'Strona główna',
        path: '/home',
        icon: <AiIcons.AiFillHome />,
    },

    {
        title: 'Twoje konto',
        path: '/userpanel',
        icon: <AiIcons.AiFillHome />,

    },

    {
        title: 'Historia',
        path: '#',
        icon: <IoIcons.IoIosPaper />,
        iconClosed: <RiIcons.RiArrowDownSFill />,
        iconOpened: <RiIcons.RiArrowUpSFill />,

        subNav: [
            {
                title: 'Użytkowników',
                path: '/user_history',
                icon: <IoIcons.IoIosPaper />,
                cName: 'sub-nav'
            },
            {
                title: 'Zleceń',
                path: '/order_history',
                icon: <IoIcons.IoIosPaper />
            },
            {
                title: 'Magazynu',
                path: '/glass_history',
                icon: <IoIcons.IoIosPaper />,
                cName: 'sub-nav'
            },
            {
                title: 'Produktów',
                path: '/ready_glass_history',
                icon: <IoIcons.IoIosPaper />
            },
            {
                title: 'Maszyn',
                path: '/all_machine_history',
                icon: <IoIcons.IoIosPaper />
            }
         
        ]
    },

    {
        title: 'Panel sterowania',
        path: '/controlpaneladmin',
        icon: <AiIcons.AiFillHome />,
    },
    {
        title: 'Zamówienia',
        path: '/orderwarehouse',
        icon: <AiIcons.AiFillHome />,

    },
    {
        title: 'Magazyn',
        path: '/glasswarehouse',
        icon: <AiIcons.AiFillHome />,

    },
    {
        title: 'Produkcja',
        path: '/selection_of_orders',
        icon: <AiIcons.AiFillHome />,

    },

    {
        title: 'Zapisane projekty',
        path: '/saved_projects',
        icon: <AiIcons.AiFillHome />,

    },

    {
        title: 'Produkty',
        path: '/ready_glass_warehouse',
        icon: <AiIcons.AiFillHome />,

    },

    {
        title: 'Maszyny',
        path: '/machinewarehouse',
        icon: <AiIcons.AiFillHome />,

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