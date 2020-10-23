import React, { Component } from 'react';
import { MDBDataTable } from 'mdbreact';
import './ReadyGlassTable.css'


export class ReadyGlassTable extends Component {
    constructor(props) {
        super(props);
        this.state = {
            table: {
                columns: [],
                rows: []
            },
        };
    }



    componentDidMount() {
        var table2 = [];
        fetch(`api/Magazine/Return_All_Glass`, {
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
                        length: json[i].length,
                        width: json[i].width,
                        thickness: json[i].hight,
                        color: json[i].color,
                        owner: json[i].owner,
                        type: json[i].type,
                        amount: json[i].glass_info.length,
                        id: '',
                        action: <button className="delate1" id={i} onClick={
                            (e) => {
                                e.preventDefault();
                                console.log(json[e.target.id].glass_info.length)
                                var amount = prompt("Podaj ilość sztuk do usunięcia z przedziału od 1 do " + json[e.target.id].glass_info.length);
                                var amount2 = parseInt(amount)
                                if (amount === null) {
                                    return;
                                }
                                else if (isNaN(amount2)) {
                                    alert("Proszę wprowadzić liczbę!");
                                    console.log(amount2)
                                }
                                else if (amount2 > json[e.target.id].glass_info.length) {
                                    alert("Wprowadź liczbę z odpowiedniego przedziału!")
                                }
                                    else { console.log(amount2) }
                            }
                        }>Usuń</button>
                    })
                };
                for (var k = 0; k < table2.length; k++) {
                    var amount = json[k].glass_info.length;
                    console.log(amount)
                    for (var j = 0; j < amount; j++) {
                        table2[k].id = table2[k].id + json[k].glass_info[j].id + ', '
                    }
                }
                this.setState({
                    table: {
                        columns: [
                            {
                                label: 'Długość',
                                field: 'length',
                                sort: 'asc',
                                width: 150
                            },
                            {
                                label: 'Szerokość',
                                field: 'width',
                                sort: 'asc',
                                width: 150
                            },
                            {
                                label: 'Grubość',
                                field: 'thickness',
                                sort: 'asc',
                                width: 150
                            },
                            {
                                label: 'Kolor',
                                field: 'color',
                                sort: 'asc',
                                width: 150
                            },
                            {
                                label: 'Rodzaj',
                                field: 'type',
                                sort: 'asc',
                                width: 150
                            },
                            {
                                label: 'Właściciel',
                                field: 'owner',
                                sort: 'asc',
                                width: 150
                            },
                            {
                                label: 'Ilość',
                                field: 'amount',
                                sort: 'asc',
                                width: 150
                            },
                            {
                                label: 'ID',
                                field: 'id',
                                sort: 'asc',
                                width: 150
                            },
                            {
                                label: 'Usuń',
                                field: 'action',
                                sort: 'asc',
                                width: 150
                            }
                        ],
                        rows: table2
                    }
                });
            })
    };

    table() {
        return (
            <MDBDataTable
                /*striped*/
                bordered
                small
                data={this.state.table}
            />
        )
    }

    render() {
        let xd = this.table();
        return (

            <div>
                {xd}
            </div>
        )
    }

}