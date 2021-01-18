import React, { Component } from 'react';
import { MDBDataTable } from 'mdbreact';
import { MDBDataTableV5 } from 'mdbreact';
import { Link } from 'react-router-dom';
import Sidebar from '../Sidebar';

export class OneOrderTable extends Component {
    constructor(props) {
        super(props);
        this.state = {
            table: {
                columns: [],
                rows: []
            },

            ids: '',
            send: [],
        };
    }



    componentDidMount() {
        const receiver = {
            order: { id_order: sessionStorage.getItem('orderId') }
        }
        fetch(`api/Order/Return_All_Items`, {
            method: "post",
            body: JSON.stringify(receiver),
            headers: {
                'Content-Type': 'application/json'
            }
        })
            .then(res => res.json())
            .then(json => {
                console.log(json)
                return (json)
            })
            .then(json => {
                var table2 = [];
                for (var i = 0; i < json.length; i++) {
                    table2.push({
                        length: json[i].length,
                        width: json[i].width,
                        thickness: json[i].thickness,
                        color: json[i].color,
                        type: json[i].type,
                        ids: json[i].id,
                        status: json[i].status,
                        desk: json[i].desk,
                        more: <Link to="/edit_order_item"><button className="prim_t" id={i}
                            onClick={
                                (e) => {
                                    /* console.log(table2[e.target.id].items);*/
                                    sessionStorage.setItem('thickness', table2[e.target.id].thickness);
                                    sessionStorage.setItem('length', table2[e.target.id].length);
                                    sessionStorage.setItem('width', table2[e.target.id].width);
                                    sessionStorage.setItem('color', table2[e.target.id].color);
                                    sessionStorage.setItem('type', table2[e.target.id].type);
                                    sessionStorage.setItem('status', table2[e.target.id].status);
                                    sessionStorage.setItem('desk', table2[e.target.id].desk);
                                    sessionStorage.setItem('itemId', table2[e.target.id].ids);
                                }
                            }>Edytuj szkło</button></Link>,
                        choice: <input type="checkbox" id={'check' + i} className={i} onClick={(e) => { this.check(e.target.id, table2[e.target.className].ids, i) }} />,
                            
                    })
                };
                this.setState({
                    table: {
                        columns: [
                            {
                                label: 'Długość',
                                field: 'length',
                                sort: 'asc',
                                width: 30
                            },
                            {
                                label: 'Szerokość',
                                field: 'width',
                                sort: 'asc',
                                width: 30
                            },
                            {
                                label: 'Grubość',
                                field: 'thickness',
                                sort: 'asc',
                                width: 30
                            },
                            {
                                label: 'Kolor',
                                field: 'color',
                                sort: 'asc',
                                width: 30
                            },
                            {
                                label: 'Rodzaj',
                                field: 'type',
                                sort: 'asc',
                                width: 30
                            },
                            {
                                label: 'Id',
                                field: 'ids',
                                sort: 'asc',
                                width: 30
                            },
                            {
                                label: 'Status',
                                field: 'status',
                                sort: 'asc',
                                width: 30
                            },
                            {
                                label: 'Edytuj',
                                field: 'more',
                                sort: 'asc',
                                width: 30
                            },
                            {
                                label: 'Zaznacz',
                                field: 'choice',
                                sort: 'asc',
                                width: 30
                            },
                        ],
                        rows: table2
                    }
                });
            })
    }


    check(number, id, miejsce) {
        var checkBox = document.getElementById(number);
        var arr = this.state.send
        if (checkBox.checked == true) {
            //alert('dodane' + '' + number)
            arr.push(id)
            this.setState.send = arr
            console.log('tablica' + '---' + this.state.send)

        } else {
            //alert('usunięte' + '' + number)
            const index = arr.indexOf(id);
            if (index > -1) {
                arr.splice(index, 1);
            }
            this.setState.send = arr
            console.log('tablica' + '---' + this.state.send)
        }
    };


    sendId = (event) => {
        event.preventDefault();
        const receiver = {
            user: { login: sessionStorage.getItem('login') },
            item_Id: this.state.send,
            order: { id_order :sessionStorage.getItem('orderId') },
        }

        fetch(`api/Order/Remove_Item`, {
            method: "post",
            body: JSON.stringify(
                receiver
            ),
            headers: {
                'Content-Type': 'application/json'
            }
        })
            .then(res => res.json())
            .then(json => {
                console.log(json)
                return (json)
            })
            .then(json => {
                alert('Przedmioty o id:' + ' ' + this.state.send + ' ' + 'zostały usunięte')
            })
            .then(json => {
                window.location.reload();
            })

        console.log(receiver)
    }

    kurwaaaaa = (event) => {
        event.preventDefault();
        console.log(this.state.send)
    }

    table() {
        return (
            <MDBDataTableV5


                hover
                entriesOptions={[10, 20, 50, 100]}
                entries={15}
                pagesAmount={10}
                data={this.state.table}
                searchTop


                materialSearch
                searchBottom={false}
                // barReverse
                //  pagingTop
                // scrollX
                // scrollY
                responsive
                // maxHeight="35vh"
                bordered



                //   maxHeight="20vh"
                // borderless
                // btn
                // dark


                //maxHeight="400px"

                // paginationLabel={["<", ">"]}

                sortable


                // small
                // tego w ciemnym trybie nie ruszać/ striped/
                // theadColor="indigo"
                theadTextWhite
                // theadColor="indigo"
                theadTextWhite
                // barReverse
                // className="User_table"
                // noBottomColumns
                sortable
            //info={false}


            //   autoWidth


            />
        )
    }

    render() {
        let table = this.table();
        return (

            <div>
                {table}
                <button onClick={this.sendId}>Usuń zaznaczone</button>
                <button onClick={this.kurwaaaaa}>kurwaaaaa</button>
            </div>
        )
    }

}