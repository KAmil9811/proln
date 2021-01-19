import React, { Component } from 'react';
import { MDBDataTable } from 'mdbreact';
import { MDBDataTableV5 } from 'mdbreact';
import { Link } from 'react-router-dom';
import Sidebar from '../Sidebar';


export class SavedOrdersTable extends Component {
    constructor(props) {
        super(props);
        this.state = {
            table: {
                columns: [],
                rows: []
            },
        };
    }




    //`api/Order/Return_All_Orders`
    componentDidMount() {
        var table2 = [];
        fetch(`api/Cut/Return_All_Project`, {
            headers: {
                'Content-Type': 'application/json',
                'Accept': 'application/json'
            }
        })
            .then(res => res.json())
            .then(json => {
                console.log(json.length);
                console.log(json);
                for (var i = 0; i < json.length; i++) {
                    table2.push({
                        number: json[i].order_id,
                        //status: json[i].status,
                        klient: json[i].owner,
                        x: json[i].stan,
                        priority: json[i].priority,
                        deadline: json[i].deadline,
                        items: json[i].items,
                        cut_id: json[i].cut_id,

                        choose: <Link to="/show_save"> <button className="success_t" id={i}
                            onClick={(e) => {
                                //this.chooseOrder( table2[e.target.id].number);
                                sessionStorage.setItem('cutId2', table2[e.target.id].cut_id)
                                sessionStorage.setItem('orderId2', table2[e.target.id].number)
                            }
                            } > Wybierz </button></Link>



                    })
                };
                this.setState({
                    table: {
                        columns: [
                            {
                                label: 'Numer referencyjny',
                                field: 'number',
                                sort: 'asc',
                                width: 30
                            },
                            {
                                label: 'Klient',
                                field: 'klient',
                                sort: 'asc',
                                width: 150
                            },
                            {
                                label: 'Priorytet',
                                field: 'priority',
                                sort: 'asc',
                                width: 30
                            },
                            {
                                label: 'Deadline',
                                field: 'deadline',
                                sort: 'asc',
                                width: 30
                            },
                            /*{
                                label: 'Status',
                                field: 'status',
                                sort: 'asc',
                                width: 30
                            },*/
                            {
                                label: 'Wybierz',
                                field: 'choose',
                                width: 30
                            }
                        ],
                        rows: table2
                    }
                });
            })
    };

    chooseOrder(id) {
        const receiver = {
            order: {
                id_order: id
            }
        }
        console.log(receiver)
        fetch(`api/Cut/Return_Package_To_Cut`, {
            method: "post",
            body: JSON.stringify(receiver),
            headers: {
                'Content-Type': 'application/json'
            }

        })


            .then(res => res.json())
            .then(json => {
                console.log(json)
                return (json);

            })
        //   .then(json => this.props.history.push('/ready_packages'))



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
                // className="Orders_table"
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
            </div>
        )
    }

}