export async function onRequest(context) {
    const ip = context.request.headers.get('CF-Connecting-IP') || 'Unknown';
    const isIPv6 = ip.includes(':');
    const protocol = isIPv6 ? 'IPv6' : 'IPv4';

    const url = new URL(context.request.url);
    const format = url.searchParams.get('format');

    const headers = {
        'Cache-Control': 'no-store',
        'X-Content-Type-Options': 'nosniff',
    };

    if (format === 'json') {
        return new Response(
            JSON.stringify({ ip: ip, Protocol: protocol }),
            { headers: { ...headers, 'Content-Type': 'application/json' } }
        );
    }

    return new Response(ip, {
        headers: { ...headers, 'Content-Type': 'text/plain' },
    });
}
