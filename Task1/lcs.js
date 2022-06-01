const arr = process.argv.slice(2)

if (arr.length === 0) {
    console.log('')
} else {
    if (arr.length === 1) {
        console.log(arr[0])
    } else {
        const str = arr[0]
        let res = ''
    
        for (let i = 0; i < str.length; i++) {
            for (let j = i + 1; j <= str.length; j++) {
                const temp = str.substring(i, j)
                let k = 1
                while (k < arr.length) { 
                    if (!arr[k].includes(temp))
                        break
                    k++
                }
                    
                if (k === arr.length && res.length < temp.length)
                    res = temp
            }
        }
    
        console.log(res)
    }
}